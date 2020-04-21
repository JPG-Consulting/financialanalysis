﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAnalyst.Common.Entities.Assets.Options
{
    [Serializable]
    public class CallOption : OptionBase
    {
        [JsonIgnore]
        public override AssetType AssetType { get { return AssetType.Option_Call; } }
    }
}
