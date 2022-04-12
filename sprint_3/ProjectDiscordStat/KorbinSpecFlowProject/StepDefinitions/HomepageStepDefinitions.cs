using KorbinSpecFlowProject.PageObjects;

namespace KorbinSpecFlowProject.StepDefinitions
{
 [Binding]
        public sealed class HomepageStepDefinitions
        {
        private readonly HomePage _homePage;

        public HomepageStepDefinitions(HomePage page)
        {
            _homePage = page;
        }
        [Given(@"I am on the home page")]
        public void GivenIAmOnTheHomePage()
        {
            _homePage.Goto();
        }

        


            [Then(@"I can see the graph")]
            public void ThenTheResultShouldBe()
            {
               int a = 1;
             
            }
        }
    }