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
    public class NanManRuQin : Aoe
    {
        public NanManRuQin()
        {
            ResponseCardVerifier = new SingleCardUsageVerifier((c) => { return c.Type is Sha; });
        }

        protected override string UsagePrompt
        {
            get { return "NanManRuQin"; }
        }
    }
}
