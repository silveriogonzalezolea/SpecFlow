﻿using SpecFlow.TestProjectGenerator.NewApi.Driver;
using TechTalk.SpecFlow.Configuration;
using TechTalk.SpecFlow.Configuration.AppConfig;
using TechTalk.SpecFlow.Specs.Drivers;

namespace TechTalk.SpecFlow.Specs.StepDefinitions
{
    [Binding]
    public class SpecFlowConfigurationSteps
    {
        private readonly AppConfigConfigurationDriver _appConfigConfigurationDriver;
        private readonly SpecFlowJsonConfigurationDriver _specFlowJsonConfigurationDriver;
        private readonly ConfigurationDriver _configurationDriver;

        public SpecFlowConfigurationSteps(AppConfigConfigurationDriver appConfigConfigurationDriver, SpecFlowJsonConfigurationDriver specFlowJsonConfigurationDriver, ConfigurationDriver configurationDriver)
        {
            this._appConfigConfigurationDriver = appConfigConfigurationDriver;
            _specFlowJsonConfigurationDriver = specFlowJsonConfigurationDriver;
            _configurationDriver = configurationDriver;
        }

        [Given(@"the specflow configuration is")]
        public void GivenTheSpecflowConfigurationIs(string specFlowConfigurationContent)
        {
            var configSection = ConfigurationSectionHandler.CreateFromXml(specFlowConfigurationContent);
            var appConfigConfigurationLoader = new AppConfigConfigurationLoader();

            var specFlowConfiguration = appConfigConfigurationLoader.LoadAppConfig(ConfigurationLoader.GetDefault(), configSection);

            _configurationDriver.SetUnitTestProvider(specFlowConfiguration.UnitTestProvider);
            _configurationDriver.SetBindingCulture(specFlowConfiguration.BindingCulture);
        }

        [Given(@"the project is configured to use the (.+) provider")]
        public void GivenTheProjectIsConfiguredToUseTheUnitTestProvider(string providerName)
        {
            _configurationDriver.SetUnitTestProvider(providerName);
        }


        [Given(@"SpecFlow is configured in the app\.config")]
        public void GivenSpecFlowIsConfiguredInTheApp_Config()
        {
            _appConfigConfigurationDriver.IsUsed = true;
        }

        [Given(@"SpecFlow is configured in the specflow\.json")]
        public void GivenSpecFlowIsConfiguredInTheSpecflow_Json()
        {
            _specFlowJsonConfigurationDriver.IsUsed = true;
        }


        [StepArgumentTransformation(@"enabled")]
        public bool ConvertEnabled() { return true; }

        [StepArgumentTransformation(@"disabled")]
        public bool ConvertDisabled() { return false; }

        [Given(@"row testing is (.+)")]
        public void GivenRowTestingIsRowTest(bool enabled)
        {
            _appConfigConfigurationDriver.SetRowTest(enabled);
        }

        [Given(@"the type '(.*)' is registered as '(.*)' in SpecFlow runtime configuration")]
        public void GivenTheTypeIsRegisteredAsInSpecFlowRuntimeConfiguration(string typeName, string interfaceName)
        {
            _appConfigConfigurationDriver.AddRuntimeDependencyCustomization(typeName, interfaceName);
        }
    }
}
