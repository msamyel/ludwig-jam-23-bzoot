using System;
namespace Bzoot
{
    public record PlayPlayerDeadAnimationArgs(bool IsRespawn, Action OnComplete);
}