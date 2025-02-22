using Buttons.EventDrivenButtons.Base;
using Buttons.Signals;

namespace Buttons.EventDrivenButtons
{
    public class OneTwoThreeOrderButton : EventDrivenButton
    {
        protected override void OnButtonClicked()
        {
            SignalBus.Fire<OneTwoThreeOrderSignal>();
        }
    }
}