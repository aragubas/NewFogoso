using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fogoso.UtilsObjects
{
    class ValueSmoother
    {
        float DeltaValue;
        float DeltaTarget;
        int Delay;
        int DelayMax;
        public float Input;
        float ValueSmooth;
        bool OtherWayAround;

        public ValueSmoother(float Smoothness, int UpdateDelay, bool pOtherWayAround=false)
        {
            ValueSmooth = Smoothness;
            DelayMax = UpdateDelay;
            OtherWayAround = pOtherWayAround;

        }

        public float GetValue()
        {
            return DeltaValue;
        }

        public void SetTargetValue(float pNewTargetValue)
        {
            Input = pNewTargetValue;
        }

        public void Update()
        {
            Delay++;

            if (Delay >= DelayMax)
            {
                Delay = 0;

                DeltaTarget = Input;
            }
            
            float IncreaseAmmount = DeltaTarget / ValueSmooth;

            if (DeltaValue < DeltaTarget) { DeltaValue += Math.Abs(IncreaseAmmount); }
            if (!OtherWayAround) { if (DeltaValue > DeltaTarget) { DeltaValue = DeltaTarget; } }
            else
            {
                if (DeltaValue > DeltaTarget) { DeltaValue -= Math.Abs(IncreaseAmmount); }

                //if (DeltaValue < DeltaTarget) { DeltaValue = DeltaTarget; }
            }


        }
    }
}
