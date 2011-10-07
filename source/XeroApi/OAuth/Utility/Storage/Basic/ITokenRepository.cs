namespace DevDefined.OAuth.Storage.Basic
{
  /// <summary>
  /// A simplistic repository for access and request of token models.
  /// </summary>    
  public interface ITokenRepository
  {
      RequestToken GetRequestToken();

      void SaveRequestToken(RequestToken requestToken);

      AccessToken GetAccessToken();

      void SaveAccessToken(AccessToken accessToken);
  }
}