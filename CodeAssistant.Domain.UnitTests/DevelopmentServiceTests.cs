using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using CodeAssistant.Domain.Services;
using CodeAssistant.Domain.Factory;
using CodeAssistant.Infrastructure;
using CodeAssistant.Domain.Execution;

namespace CodeAssistant.Domain.UnitTests
{
    [TestFixture]
    public class DevelopmentServiceTests
    {
        [Test]
        public void ExecuteCodeTest()
        {
            var factory = new ServiceFactory(
                FileProvider.Provider, 
                new MockLanguageRepository());

            var executionService = factory.GetExecutionService();
            var languageService = factory.GetLanguageService();
            var sourceCodeFactory = new SourceCodeFactory(FileProvider.Provider);

            var language = languageService.GetLanguage("Php");

            string code = "<?php\n\techo \"Hello world\";\n?>";
            string expected = "Hello world";
            string actual = null;

            var sourceCode = sourceCodeFactory.GetSourceCode(language, code);

            executionService.ExecutionDataEvent += (s, a) =>
            {
                if (a.IsComplete)
                {
                    actual = a.Output + a.Error;
                }
            };

            executionService.ExecuteCode(sourceCode);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestGetExecutionService()
        {
            var provider = FileProvider.Provider;
            Assert.IsNotNull(provider);

            var factory = new ServiceFactory(provider, null);
            Assert.IsNotNull(factory);
            
            var service = factory.GetFileService();
            Assert.IsNotNull(service);
        }

        //[Test]
        //public void ExecutionFlowTest()
        //{
        //    var sourceCode = TestHelper.GetSourceCode();
        //    var strategy = ExecutionStrategyFactory.Create(sourceCode);
        //    Assert.IsTrue(strategy is InterpretedExecutionStrategy, "Not interpreted execution strategy");

        //    var results = strategy.GetExecutable();
        //    Assert.IsNotNull(results);
        //    Assert.IsTrue(results is Runtime, "Not runtime");

        //    var output = results.Execute();
        //    Assert.IsNotNull(output);
        //    Assert.AreEqual(executionResults, output);
        //}

        //[TestMethod]
        //public void ExecutionStrategyFactoryTests()
        //{
        //    var sourceCode = TestHelper.GetSourceCode();
        //    var strategy = ExecutionStrategyFactory.Create(sourceCode);

        //    Assert.IsNotNull(strategy);
        //    Assert.IsTrue(strategy is InterpretedExecutionStrategy, "Not Interpreted Execution Strategy");
        //}

        //#region Language Construction

        //[TestMethod]
        //public void LanguageTypeFactoryTest()
        //{
        //    var interp = LanguageTypeFactory.GetLanguage(Domain.LanguageType.Interpreted);
        //    var compiled = LanguageTypeFactory.GetLanguage(Domain.LanguageType.Compiled);
        //    Assert.IsTrue(interp is InterpretedLanguage, "Not interpreted language");
        //    Assert.IsTrue(compiled is CompiledLanguage, "Not compiled language");
        //}

        //[TestMethod()]
        //public void LanguageDirectorConstructorTest()
        //{
        //    LanguageDirector target = new LanguageDirector();
        //    Assert.IsNotNull(target);
        //}

        ///// <summary>
        /////A test for Build
        /////</summary>
        //[TestMethod()]
        //public void BuildTest()
        //{
        //    LanguageDirector target = new LanguageDirector();
        //    Language materials = TestHelper.GetGenericLanguage();
        //    LanguageBase product = LanguageTypeFactory.GetLanguage(materials.Type);
        //    LanguageBase expected = TestHelper.GetLanguage();
        //    LanguageBase actual;

        //    actual = target.Build(materials, ref product);
        //    Assert.IsTrue(expected.GetHashCode() == actual.GetHashCode(), "Results are not equal");
        //}

    }
}
