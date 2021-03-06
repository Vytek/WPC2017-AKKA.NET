﻿using Akka.Actor;
using System;

namespace AKKA.Library.Demo
{
    public class SenderUntypedActor : UntypedActor
    {
        private IActorRef _firstUntyped;

        public IActorRef FirstUntyped
        {
            get
            {
                if (_firstUntyped == null)
                    _firstUntyped = Context.ActorSelection($"/user/FirstUntypedActor")
                        .ResolveOne(TimeSpan.FromSeconds(2))
                        .Result;

                return _firstUntyped;
            }
            private set { _firstUntyped = value; }
        }

        private IActorRef _firstTestUntyped;

        public IActorRef FirstTestUntyped
        {
            get
            {
                if (_firstTestUntyped == null)
                    _firstTestUntyped = Context.ActorSelection($"../FirstUntypedActor")
                        //_firstTestUntyped = Context.ActorSelection($"akka://test/system/testActor1/FirstUntypedActor")
                        .ResolveOne(TimeSpan.FromSeconds(2))
                        .Result;

                return _firstTestUntyped;
            }
            private set { _firstTestUntyped = value; }
        }

        public SenderUntypedActor()

        {
        }

        public SenderUntypedActor(IActorRef firstUntyped)
        {
            FirstUntyped = firstUntyped;
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case SimpleMessage msg:
                    HandleSimpleMessage(msg);
                    break;

                case ForwardMessage msg:
                    HandleForwardMessage(msg);
                    break;

                default:
                    break;
            }
        }

        public static Props Props(IActorRef firstUntyped)
        {
            return Akka.Actor.Props.Create(() => new SenderUntypedActor(firstUntyped));
        }

        private void HandleSimpleMessage(SimpleMessage msg)
        {
            Console.WriteLine($"Message:{msg.Value} " +
                              $"\nreceived by {Context.Self.Path}" +
                              $"\nsender {Sender.Path}" +
                              $"\nforwarded to {FirstUntyped.Path}" +
                              $"\n");
            FirstUntyped.Tell(msg);
        }

        private void HandleForwardMessage(ForwardMessage msg)
        {
            Console.WriteLine($"Message:{msg.Value} " +
                              $"\nreceived by {Context.Self.Path}" +
                              $"\nsender {Sender.Path}" +
                              $"\nforwarded to {FirstTestUntyped.Path}" +
                              $"\n");
            FirstTestUntyped.Forward(msg);
        }
    }
}