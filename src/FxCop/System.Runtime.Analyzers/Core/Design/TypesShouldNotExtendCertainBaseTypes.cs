using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace System.Runtime.Analyzers.Design
{
    [DiagnosticAnalyzer(LanguageNames.CSharp, LanguageNames.VisualBasic)]
    class TypesShouldNotExtendCertainBaseTypes : DiagnosticAnalyzer
    {
        internal const string RuleTypesShouldNotExtendCertainBaseType = "CA1058";
        internal const string RuleNameForExportAttribute = "TypesShouldNotExtendCertainBaseTypes";

        private static readonly List<string> s_badBaseTypes = new List<string>
                                                    {
                                                        "System.ApplicationException",
                                                        "System.Xml.XmlDocument",
                                                        "System.Collections.CollectionBase",
                                                        "System.Collections.DictionaryBase",
                                                        "System.Collections.Queue",
                                                        "System.Collections.ReadOnlyCollectionBase",
                                                        "System.Collections.SortedList",
                                                        "System.Collections.Stack"
                                                    };
        private static readonly LocalizableString s_localizableTitleCA1058 = new LocalizableResourceString(nameof(SystemRuntimeAnalyzersResources.TypesShouldNotExtendCertainBaseTypes), SystemRuntimeAnalyzersResources.ResourceManager, typeof(SystemRuntimeAnalyzersResources));
        private static readonly LocalizableString s_localizableMessageCA1058 = new LocalizableResourceString(nameof(SystemRuntimeAnalyzersResources.TypesShouldNotExtendCertainBaseTypes), SystemRuntimeAnalyzersResources.ResourceManager, typeof(SystemRuntimeAnalyzersResources));
        private static readonly LocalizableString s_localizableDescriptionCA1058 = new LocalizableResourceString(nameof(SystemRuntimeAnalyzersResources.TypesShouldNotExtendCertainBaseTypesDescription), SystemRuntimeAnalyzersResources.ResourceManager, typeof(SystemRuntimeAnalyzersResources));
        private static DiagnosticDescriptor s_rule1058 = new DiagnosticDescriptor(RuleTypesShouldNotExtendCertainBaseType,
                                                                             s_localizableTitleCA1058,
                                                                             s_localizableMessageCA1058,
                                                                             DiagnosticCategory.Design,
                                                                             DiagnosticSeverity.Warning,
                                                                             isEnabledByDefault: false,
                                                                             description: s_localizableDescriptionCA1058,
                                                                             helpLinkUri: "https://msdn.microsoft.com/en-us/library/ms182171.aspx",
                                                                             customTags: WellKnownDiagnosticTags.Telemetry);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(s_rule1058);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterCompilationStartAction(AnalyzeCompilationStart);
        }

        private void AnalyzeCompilationStart(CompilationStartAnalysisContext context)
        {
            var badBaseTypes = s_badBaseTypes
                                .Select(bt => context.Compilation.GetTypeByMetadataName(bt))
                                .Where(bt => bt != null)
                                .ToList();

            context.RegisterSymbolAction((saContext) =>
                {
                    var namedTypeSymbol = saContext.Symbol as INamedTypeSymbol;
                    if (!namedTypeSymbol.Locations.Any(l => l.IsInSource))
                    {
                        return;
                    }

                    var containingBadBaseTypes = badBaseTypes.Where(bbt => bbt.Equals(namedTypeSymbol.BaseType));

                    if (containingBadBaseTypes.Any())
                    {
                        saContext.ReportDiagnostic(namedTypeSymbol.CreateDiagnostic(s_rule1058, namedTypeSymbol.ToDisplayString(), containingBadBaseTypes.First().ToDisplayString()));
                    }
                }
                , SymbolKind.NamedType);
        }

        private void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            throw new NotImplementedException();
        }
    }
}
