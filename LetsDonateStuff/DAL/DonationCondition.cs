using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace LetsDonateStuff.DAL
{
    public enum DonationCondition
    {
        [Description("New")]
        New,
        [Description("Little Used")]
        LittleUsed,
        [Description("Used")]
        Used,
        [Description("Needs Repair")]
        NeedsRepair,
    }
}