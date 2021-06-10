using System;
using Xunit;
using Xunit.Abstractions;

namespace TestProject1
{
    public class UnitTest1
    {
        public UnitTest1(ITestOutputHelper output)
        {
            output.WriteLine(DateTime.Now.ToString());
        }
        [Theory]
        [InlineData("abcd","aba", true)]
        public void GetAccountById_ExpectRightResult_WithoutException(string title, string content, bool expectedResult)
        {
            bool actualResult = new Nextflip.Models.notification.NotificationDAO().AddNotification(title, content);
            ///assert
            Assert.Equal(expectedResult, actualResult);
        }
    }
}
