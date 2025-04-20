using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetArchTest.Rules;
using TestResult = NetArchTest.Rules.TestResult;

namespace FitnessFunctions;

[TestClass]
public class DependencyTests
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
}
