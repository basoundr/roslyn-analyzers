using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Analyzers.Design;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.UnitTests;
using Xunit;

namespace System.Runtime.Analyzers.UnitTests.Design
{
    public class TypesShouldNotExtendCertainBaseTypesTests : DiagnosticAnalyzerTestBase
    {
        protected override DiagnosticAnalyzer GetBasicDiagnosticAnalyzer()
        {
            return new TypesShouldNotExtendCertainBaseTypes();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new TypesShouldNotExtendCertainBaseTypes();
        }

        [Fact]
        public void TestCSSimpleAttributeClass()
        {
            VerifyCSharp(@"
using System;

class C : Attribute
{
}
");
        }
    }
}
