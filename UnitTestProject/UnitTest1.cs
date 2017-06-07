using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using CTA.Repositories;
using Microsoft.AspNetCore.Identity;
using CTA.Controllers.api;
using Microsoft.AspNetCore.Mvc;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SessionControllerReturn200IfAccountExisted()
        {
            var signInManager = Substitute.For<ISignInManager>();
            signInManager.Login(Arg.Any<string>(), Arg.Any<string>(), 
                Arg.Any<bool>(), Arg.Any<bool>())
                .Returns(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var controller = new SessionController(signInManager);

            ActionResult result = controller.Login(new CTA.ViewModels.LoginUserModel()
            { UserName = "testuserr", Password = "pa$$w0rd", RememberMe = true });


            Assert.IsInstanceOfType(result,typeof(OkObjectResult));
        }

        [TestMethod]
        public void SessionControllerReturn400IfErrors()
        {
            var signInManager = Substitute.For<ISignInManager>();
            signInManager.Login(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(Microsoft.AspNetCore.Identity.SignInResult.Success);

            var controller = new SessionController(signInManager);

            ActionResult result = controller.Login(new CTA.ViewModels.LoginUserModel() { UserName = "testuserr", Password = "dfdfdf", RememberMe = true });


            Assert.IsInstanceOfType(result, typeof(ObjectResult));
        }
    }
}
