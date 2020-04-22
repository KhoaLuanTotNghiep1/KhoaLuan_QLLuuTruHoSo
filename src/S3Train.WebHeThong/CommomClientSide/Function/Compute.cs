using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S3Train.WebHeThong.CommomClientSide.Function
{
    public static class Compute
    {
        public static int ComputeAmountWithAction(int amount, ActionWithObject action)
        {
            int result = 0;
            switch(action)
            {
                case ActionWithObject.Create:
                    result = amount;
                    break;
                case ActionWithObject.Update:
                    result = amount + 1;
                    break;
                case ActionWithObject.Delete:
                    result = amount - 1;
                    break;
                default:
                    break;
            }
            return result;
        }
    }
}