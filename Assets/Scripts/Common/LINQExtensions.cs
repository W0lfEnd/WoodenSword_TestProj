using System.Collections.Generic;


public static class LINQExtensions
{
  public static void SetOrIncrement<TKey>( this IDictionary<TKey, int> dict, TKey key, int value = 1 )
  {
    dict.TryGetValue( key, out int cur_val );
    dict[key] = cur_val + value;
  }

  public static void SetOrIncrement<TKey>( this IDictionary<TKey, float> dict, TKey key, float value = 1.0f )
  {
    dict.TryGetValue( key, out float cur_val );
    dict[key] = cur_val + value;
  }
}