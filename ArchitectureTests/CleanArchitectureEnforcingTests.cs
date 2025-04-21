using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetArchTest.Rules;
using TestResult = NetArchTest.Rules.TestResult;

namespace ArchitectureTests;

[TestClass]
public class CleanArchitectureEnforcingTests
{
    private static readonly Assembly s_domain = Assembly.Load("Domain");
    private static readonly Assembly s_application = Assembly.Load("Application");
    private static readonly Assembly s_infrastructure = Assembly.Load("Infrastructure");

    [TestMethod]
    public void DomainShouldNotHaveDependenciesOnOtherLayers()
    {
        TestResult result = Types.InAssembly(s_domain)
            .ShouldNot()
            .HaveDependencyOnAny("Application", "Infrastructure", "Web.API")
            .GetResult();
        Assert.IsTrue(result.IsSuccessful, $"Domain has invalid dependencies: {string.Join(", ", result)}");
    }

    [TestMethod]
    public void ApplicationShouldOnlyDependOnDomain()
    {
        TestResult result = Types.InAssembly(s_application)
            .ShouldNot()
            .HaveDependencyOnAny("Infrastructure", "Web.API")
            .GetResult();
        Assert.IsTrue(result.IsSuccessful, $"Application has invalid dependencies: {string.Join(", ", result)}");
    }

    [TestMethod]
    public void InfrastructureShouldNotDependOnApi()
    {
        TestResult result = Types.InAssembly(s_infrastructure)
            .ShouldNot()
            .HaveDependencyOnAll("Web.API")
            .GetResult();
        Assert.IsTrue(result.IsSuccessful, $"Infrastructure has invalid dependencies: {string.Join(", ", result)}");
    }

    [TestMethod]
    public void ShouldNotHaveUtilsInNamespace()
    {
        IEnumerable<IType> result = Types
            .InAssemblies([s_domain, s_application, s_infrastructure])
            .That()
            .ResideInNamespaceContaining("Utils")
            .Or()
            .ResideInNamespaceContaining("Helpers")
            .GetTypes();

        Assert.IsEmpty(result, """
                               Classes não devem estar em namespaces contendo 'Utils'.

                               Evite agrupar funcionalidades genéricas ou sem coesão em pastas como 'Utils' ou 'Helpers'.

                               Exemplo:
                               Em vez de StringUtils.FormatCpf(), prefira CpfFormatter.Format()
                               Em vez de DateUtils.IsWeekend(), prefira WorkSchedule.IsWeekend(DateTime date)
                               """);
    }
}
