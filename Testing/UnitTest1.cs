using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Testing;

public class Tests
{
    IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        //instatiate webdriver
        driver = new ChromeDriver();
        driver.Navigate().GoToUrl("https://saucedemo.com");
        Thread.Sleep(2000);
    }


    [Test] //succesful login
    [TestCaseSource(nameof(Usernames))]
    public void Test_login(String username)
    {
        //navgiate through login page
        driver.FindElement(By.XPath("//input[@id='user-name']")).SendKeys(username);
        Thread.Sleep(1000);
        driver.FindElement(By.XPath("//input[@id='password']")).SendKeys("secret_sauce");
        Thread.Sleep(1000);
        driver.FindElement(By.XPath("//input[@id='login-button']")).Click();
        Thread.Sleep(1000);


    }

    [Test]  //there is a store page with one or more items available for purchase
    [TestCaseSource(nameof(Usernames))]
    public void Test_product_page(String username)
    {
        //navigate through login page
        driver.FindElement(By.XPath("//input[@id='user-name']")).SendKeys(username);
        Thread.Sleep(1000);
        driver.FindElement(By.XPath("//input[@id='password']")).SendKeys("secret_sauce");
        Thread.Sleep(1000);
        driver.FindElement(By.XPath("//input[@id='login-button']")).Click();
        Thread.Sleep(1000);

        //test if login was successful first 
        String url = driver.Url;
        Assert.That(url, Is.EqualTo("https://www.saucedemo.com/inventory.html"));

        //find item names from page
        String item1 = driver.FindElement(By.XPath("//a[@id='item_1_title_link']/div[@class='inventory_item_name']")).Text;
        String item2 = driver.FindElement(By.XPath("//a[@id='item_4_title_link']/div[@class='inventory_item_name']")).Text;

        //more than one item is present on the page
        Assert.Multiple(() =>
        {
            Assert.That(item1, Is.Not.Empty);
            Assert.That(item2, Is.Not.Empty);
        });
    }

    [Test]  //try to add items to cart
    [TestCaseSource(nameof(Usernames))]
    public void Test_purchasing(String username)
    {
        //navigate through login page
        driver.FindElement(By.XPath("//input[@id='user-name']")).SendKeys(username);
        Thread.Sleep(1000);
        driver.FindElement(By.XPath("//input[@id='password']")).SendKeys("secret_sauce");
        Thread.Sleep(1000);
        driver.FindElement(By.XPath("//input[@id='login-button']")).Click();
        Thread.Sleep(1000);

        //test if login was successful first
        String url = driver.Url;
        Assert.That(url, Is.EqualTo("https://www.saucedemo.com/inventory.html"));

        //try to add item to cart
        driver.FindElement(By.XPath("//button[@data-test='add-to-cart-sauce-labs-bolt-t-shirt']")).Click();
        Thread.Sleep(1000);
        //check if add cart button changed to remove 
        Boolean removeButton = driver.FindElements(By.XPath("//button[@data-test='remove-sauce-labs-bolt-t-shirt']")).Count > 0;
        Assert.That(removeButton, Is.True);
    }

    [TearDown]
    public void CloseBrowser()
    {
        driver.Close();
    }

    //testcase source
    static readonly object[] Usernames =
    {
        new object[] {"standard_user"},
        new object[] {"problem_user"},
        new object[] {"locked_out_user"}
    };

}