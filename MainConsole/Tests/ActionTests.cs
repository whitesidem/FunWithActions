using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace MainConsole.Tests
{

    [TestFixture]
    public class ActionTests
    {

        [Test]
        public void UseBuilderToCreateActivationEvent()
        {

            var result = new ActionBuilder<ActivateAction>()
                .WithCode("HAPPY")
                .WithClient(8)
                .Build( () => new ActivateAction());

            //Assert
            Assert.That(result.Code, Is.EqualTo("HAPPY"));
            Assert.That(result.Client, Is.EqualTo(8));

        }

        [Test]
        public void UseBuilderToCreateDeactivationEvent()
        {

            var result = new ActionBuilder<DeactivateAction>()
                .WithCode("SAD")
                .WithClient(2)
                .Build(() => new DeactivateAction());

            result.Wazuuup = "Oh yeah - generics!";

            //Assert
            Assert.That(result.Code, Is.EqualTo("SAD"));
            Assert.That(result.Client, Is.EqualTo(2));

        }

    }

    public class ActionBuilder<T> where T:IPromoAction
    {
        private readonly IList<Action<IPromoAction>> actions = new List<Action<IPromoAction>>();

        public ActionBuilder<T> WithCode(string code)
        {
            actions.Add(x =>
            {
                x.Code = code;
            });
            return this;
        }

        public ActionBuilder<T> WithClient(int client)
        {
            actions.Add(x =>
            {
                x.Client = client;
            });
            return this;
        }

        public T Build(Func<T> createFunc)
        {
            var instance = createFunc.Invoke();
            foreach (var action in actions)
            {
                action(instance);
            }
            return instance;
        }
    }

    public class ActivateAction : IPromoAction
    {
        public string Code { get; set; }
        public int Client { get; set; }
    }

    public class DeactivateAction : IPromoAction
    {
        public string Code { get; set; }
        public int Client { get; set; }
        public string Wazuuup { get; set; }
    }


    public interface IPromoAction
    {
        string Code { get; set; }
        int Client { get; set; }
    }
}
