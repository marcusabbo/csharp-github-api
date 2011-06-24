﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using RestSharp;

namespace csharp_github_api.IntegrationTests.Models
{
    [TestFixture]
    public class UserTests
    {
        private Github _github;

        [TestFixtureSetUp]
        public void Setup()
        {
            var secrets = new SecretsHandler(@"C:\development\secretpasswordfile.xml");
            var authenticator = new HttpBasicAuthenticator(secrets.Username, secrets.Password);
            _github = new Github("https://api.github.com", authenticator);    
        }

        [Test]
        public void Should_Get_List_Of_Following()
        {
            var result = _github.User.GetUser("sgrassie").Following;

            Assert.That(result, Has.Count.GreaterThan(0));
        }

        [Test]
        public void Should_Get_List_Of_Followers()
        {
            var result = _github.User.GetUser("sgrassie").Followers;

            Assert.That(result, Has.Count.GreaterThan(0));
        }

        [Test]
        public void Should_Follow_And_Unfollow_User()
        {
            var user =  _github.User.GetUser("sgrassie");

            user.Authenticated.Follow("mono").Should().BeTrue("should follow the user mono");

            user.Following.Should().Contain(x => x.Login == "mono");

            user.Authenticated.UnFollow("mono").Should().BeTrue("Should unfollow the user mono");

            user.Following.Should().NotContain(x => x.Login == "mono");
        }
    }
}
