
namespace CAM.Core.Business.Interface
{
    public interface IBaseInterfaceCommand<TDBContext> : IMixinInterface
    {
        TDBContext DBContext { get; }
    }
}
