using DynamicData;
using YaDiskBackup.Domain.Models;

namespace YaDiskBackup.Domain.Abstractions;

public interface IBackup
{
    void Enable();

    IObservableCache<CopiedFile, long> Live { get; }

    void Disable();
}