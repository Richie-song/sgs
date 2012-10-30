﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Sanguosha.Core.UI;
using Sanguosha.Core.Skills;
using Sanguosha.Core.Players;
using Sanguosha.Core.Games;
using Sanguosha.Core.Triggers;
using Sanguosha.Core.Exceptions;
using Sanguosha.Core.Cards;

namespace Sanguosha.Expansions.Basic.Cards
{
    public class CiXiongShuangGuJian : Weapon
    {
        private Trigger trigger1;

        protected override void RegisterWeaponTriggers(Player p)
        {
            trigger1 = new CiXiongShuangGuJianTrigger(p);
            Game.CurrentGame.RegisterTrigger(Sha.PlayerShaTargetModifier, trigger1);
        }

        protected override void UnregisterWeaponTriggers(Player p)
        {
            Game.CurrentGame.UnregisterTrigger(Sha.PlayerShaTargetModifier, trigger1);
            trigger1 = null;
        }

        protected override void Process(Player source, Player dest, ICard card)
        {
            throw new NotImplementedException();
        }

        class CiXiongShuangGuJianTrigger : Trigger
        {
            public override void Run(GameEvent gameEvent, GameEventArgs eventArgs)
            {
                Trace.Assert(eventArgs.Targets.Count == 1);
                Trace.TraceInformation("CiXiong trigger");
                if (eventArgs.Source == Owner)
                {
                    if ((eventArgs.Targets[0].IsFemale && eventArgs.Source.IsMale) ||
                        (eventArgs.Targets[0].IsMale && eventArgs.Source.IsFemale))
                    {
                        int answer;
                        if (Game.CurrentGame.UiProxies[eventArgs.Source].AskForMultipleChoice(new Prompt("CiXiong"), Prompt.YesNoChoices, out answer) && answer == 0)
                        {
                            ISkill skill;
                            List<Card> cards;
                            List<Player> players;
                            SingleCardDiscardVerifier v = new SingleCardDiscardVerifier();
                            if (!Game.CurrentGame.UiProxies[eventArgs.Targets[0]].AskForCardUsage(new Prompt("CiXiong2", eventArgs.Source), v, out skill, out cards, out players))
                            {
                                Game.CurrentGame.DrawCards(eventArgs.Source, 1);
                            }
                            else
                            {
                                Game.CurrentGame.HandleCardDiscard(eventArgs.Targets[0], cards);
                            }
                        }
                    }
                }

            }
            public CiXiongShuangGuJianTrigger(Player p)
            {
                Owner = p;
            }
        }


        public override int AttackRange
        {
            get { return 2; }
        }
    }
}
