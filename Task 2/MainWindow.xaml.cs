using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Task_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    //С помощью Selenium WebDriver выполнить следующую процедуру:
    //- Зайти на страницу https://careers.veeam.ru/vacancies и развернуть браузер во весь экран.
    //- Из списка отделов выбрать Разработка продуктов, из списка языков выбрать Английский.
    //- Подсчитать количество выданных вакансий и сравнить с ожидаемым результатом.
    //Примечания:
    //- Желательно параметризовать значения входных параметров и ожидаемого количества вакансий, чтобы этот код можно было использовать для разного набора параметров.
    //- Браузер можно использовать любой.

    public partial class MainWindow : Window
    {
        IWebDriver browser;
        string coockiesBtnId = "cookiescript_accept";
        string vacanciesSelectorXPath = "//*[@id=\"root\"]/div/div[1]/div/div[2]/div[2]/div/a";
        string langSelCssSelector = "#root > div > div.container-main.container-fluid > div > " +
            "div.row.block-spacer-top > div.col-12.col-lg-4 > div > div:nth-child(3) > div > div > #sl";
        string langOptionXPath = "//*[@id=\"root\"]/div/div[1]/div/div[2]/div[1]/div/div[3]/div/div/div/div";
        string departmentCssSelector = "#root > div > div.container-main.container-fluid > div > " +
            "div.row.block-spacer-top > div.col-12.col-lg-4 > div > div:nth-child(2) > div > div";
        string departmentsCssSelector = "#root > div > div.container-main.container-fluid > div > " +
            "div.row.block-spacer-top > div.col-12.col-lg-4 > div > div:nth-child(2) > div > div > div";
        public MainWindow()
        {
            InitializeComponent();
            browser = new ChromeDriver();
            browser.Manage().Window.Maximize();
            browser.Navigate().GoToUrl("https://careers.veeam.ru/vacancies");
        }

        private void openBrowserBtn_Click(object sender, RoutedEventArgs e)
        {
            HideCoockiesBtn();
            FillDepartment();
            FillLanguage();
            var vacancies = browser.FindElements(By.XPath(vacanciesSelectorXPath)).ToList();
            actualVacNumTextBox.Text = "" + vacancies.Count;
            differenceVacNumTextBox.Text = "" + (vacancies.Count - int.Parse(expectedVacNumTextBox.Text));
        }

        private void HideCoockiesBtn() 
        {
            var btn = browser.FindElement(By.Id(coockiesBtnId));
            if (btn.Displayed) btn.Click();
        }

        private void FillLanguage()
        {
            var languageSelector = browser.FindElement(By.CssSelector(langSelCssSelector));
            if (languageSelector.Text != languageTextBox.Text)
            {
                try { languageSelector.Click(); }
                catch (System.Exception) { return; }
                var languages = browser.FindElements(By.XPath(langOptionXPath))
                    .ToList();

                UnselectLanguages(languages);

                try
                {
                    for (int i = 0; i < languages.Count; i++)
                    {
                        var lang = languages[i];
                        var langLabel = lang.FindElement(By.TagName("label"));
                        if (langLabel.Text == languageTextBox.Text) langLabel.Click();
                    }
                }
                catch (System.Exception) { MessageBox.Show("Данного языка нет в списке языков"); }
                finally { languageSelector.Click(); }
            }
        }

        private void UnselectLanguages(List<IWebElement> languages)
        {
            foreach (var lang in languages)
            {
                var isChecked = lang.FindElement(By.TagName("input")).GetAttribute("checked");
                if (isChecked == "true") lang.FindElement(By.TagName("label")).Click();
            }
        }

        private void FillDepartment()
        {
            var departmentSelector = browser.FindElement(By.CssSelector(departmentCssSelector));
            if (departmentSelector.Text != departmentTextBox.Text)
            {
                departmentSelector.Click();
                try
                {
                    var departments = browser.FindElement(By.CssSelector(departmentsCssSelector));
                    var department = departments.FindElement(By.LinkText(departmentTextBox.Text));
                    if (department != null) department.Click();
                }
                catch (System.Exception) { MessageBox.Show("Данного отдела нет в списке отделов"); }
            }
        }
    }
}
