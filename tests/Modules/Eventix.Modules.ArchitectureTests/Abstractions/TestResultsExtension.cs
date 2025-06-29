using FluentAssertions;
using NetArchTest.Rules;

namespace Eventix.Modules.ArchitectureTests.Abstractions
{
    internal static class TestResultsExtension
    {
        internal static void ShouldBeSuccessful(this TestResult testResult)
        {
            testResult.FailingTypes?.Should().BeEmpty();
        }
    }
}