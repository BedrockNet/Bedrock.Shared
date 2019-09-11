namespace Bedrock.Shared.Mapper.Interface
{
    public interface IMapper
    {
        #region Methods
        TTo Map<TFrom, TTo>(object to, params object[] from);

        TTo Flatten<TFrom, TTo>(object to, params object[] from);

        TTo Unflatten<TFrom, TTo>(object to, params object[] from);
        #endregion
    }
}
