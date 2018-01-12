using System.Collections.Generic;
using System.Threading.Tasks;
using Floxdc.ExponentServerSdk.Response;

namespace Floxdc.ExponentServerSdk
{
    public interface IPushClient
    {
        Task<PushResponse> Publish(PushMessage pushMessage);
        Task<IReadOnlyCollection<PushResponse>> PublishMultiple(IEnumerable<PushMessage> pushMessages);
    }
}