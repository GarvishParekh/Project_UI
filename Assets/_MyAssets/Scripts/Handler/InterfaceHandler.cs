public interface IButtonAnimation
{
    public void IsPressed();
    public void IsReleased();
    public void IsCompleted();
    public void ResetAnimation(bool check);
}