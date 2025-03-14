using YG;
using System;

public class ShowRewardButton : DefaultButton
{
    public event Action<int> IsShown;

    protected override void OnClick()
    {
        YandexGame.RewVideoShow((int)RewardType.UnlockParkingPlace);
        YandexGame.RewardVideoEvent += AfterReward;
    }

    private void AfterReward(int id)
    {
        if (id != (int)RewardType.UnlockParkingPlace)
            return;

        YandexGame.RewardVideoEvent -= AfterReward;
        IsShown?.Invoke(id);
    }
}
