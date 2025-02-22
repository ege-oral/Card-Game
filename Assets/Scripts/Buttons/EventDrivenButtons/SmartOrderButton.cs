using Buttons.EventDrivenButtons.Base;
using Buttons.Signals;

namespace Buttons.EventDrivenButtons
{
    public class SmartOrderButton : EventDrivenButton
    {
        protected override void OnButtonClicked()
        {
            SignalBus.Fire<SmartOrderSignal>();
        }
    }
}