namespace Aivis;

/// <summary>
/// AIVMモデルを検索するためのオプションを定義するクラス。
/// </summary>
public class SearchVoiceModelsOptions
{
    /// <summary>
    /// 検索キーワード
    /// </summary>
    public string? Keyword { get; set; }

    private List<string> _tags = new();

    /// <summary>
    /// タグ
    /// </summary>
    public string[]? Tags
    {
        get
        {
            if (_tags.Count == 0)
            {
                return null;
            }
            return _tags.ToArray();
        }
        set
        {
            _tags = value?.Select(t => t.Trim()).ToList() ?? new List<string>();
        }
    }

    private List<string> _categories = new();

    /// <summary>
    /// カテゴリ
    /// </summary>
    public string[]? Categories
    {
        get
        {
            if (_categories.Count == 0)
            {
                return null;
            }
            return _categories.ToArray();
        }
        set
        {
            _categories = value?.Select(c => c.Trim()).ToList() ?? new List<string>();
        }
    }

    private List<string> _voiceTimbres = new();

    /// <summary>
    /// 声の音質
    /// </summary>
    public string[]? VoiceTimbres
    {
        get
        {
            if (_voiceTimbres.Count == 0)
            {
                return null;
            }
            return _voiceTimbres.ToArray();
        }
        set
        {
            _voiceTimbres = value?.Select(t => t.Trim()).ToList() ?? new List<string>();
        }
    }

    private List<string> _licenceTypes = new();

    /// <summary>
    /// ライセンス種類
    /// </summary>
    public string[]? LicenceTypes
    {
        get
        {
            if (_licenceTypes.Count == 0)
            {
                return null;
            }
            return _licenceTypes.ToArray();
        }
        set
        {
            _licenceTypes = value?.Select(t => t.Trim()).ToList() ?? new List<string>();
        }
    }


    /// <summary>
    /// ソートする項目
    /// </summary>
    public string Sort { get; set; } = "download";

    /// <summary>
    /// 表示するページ番号
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// 1ページに表示する項目数
    /// </summary>
    public int Limit { get; set; } = 24;

    /// <summary>
    /// SearchVoiceModelsOptions クラスのコンストラクタ。
    /// </summary>
    public SearchVoiceModelsOptions()
    {
    }

    /// <summary>
    /// キーワードを設定します。
    /// </summary>
    /// <param name="keyword">検索キーワード</param>
    /// <returns>このオプションオブジェクト自身</returns>
    public SearchVoiceModelsOptions SetKeyword(string? keyword)
    {
        Keyword = keyword?.Trim();
        return this;
    }

    /// <summary>
    /// タグを設定します。
    /// </summary>
    /// <param name="tags">タグ</param>
    /// <returns>このオプションオブジェクト自身</returns>
    public SearchVoiceModelsOptions SetTags(params string[] tags)
    {
        Tags = tags?.Select(t => t.Trim()).ToArray();
        return this;
    }

    /// <summary>
    /// カテゴリを設定します。
    /// </summary>
    /// <param name="categories">カテゴリ</param>
    /// <returns>このオプションオブジェクト自身</returns>
    public SearchVoiceModelsOptions SetCategories(params string[] categories)
    {
        Categories = categories?.Select(c => c.Trim()).ToArray();
        return this;
    }

    /// <summary>
    /// 声の音質を設定します。
    /// </summary>
    /// <param name="voiceTimbres">声の音質</param>
    /// <returns>このオプションオブジェクト自身</returns>
    public SearchVoiceModelsOptions SetVoiceTimbres(params string[] voiceTimbres)
    {
        VoiceTimbres = voiceTimbres?.Select(t => t.Trim()).ToArray();
        return this;
    }

    /// <summary>
    /// ライセンス種別を設定します。
    /// </summary>
    /// <param name="licenceTypes">ライセンス種別</param>
    /// <returns>このオプションオブジェクト自身</returns>
    public SearchVoiceModelsOptions SetLicenceTypes(params string[] licenceTypes)
    {
        LicenceTypes = licenceTypes?.Select(t => t.Trim()).ToArray();
        return this;
    }

    /// <summary>
    /// ソート対象を設定します。
    /// </summary>
    /// <param name="sort">ソート対象</param>
    /// <returns>このオプションオブジェクト自身</returns>
    public SearchVoiceModelsOptions SetSort(string sort)
    {
        Sort = sort.Trim();
        return this;
    }

    /// <summary>
    /// ページ番号を設定します。
    /// </summary>
    /// <param name="page">ページ番号</param>
    /// <returns>このオプションオブジェクト自身</returns>
    public SearchVoiceModelsOptions SetPage(int page)
    {
        if (page < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than or equal to 1.");
        }
        Page = page;
        return this;
    }

    /// <summary>
    /// 1ページあたりの件数を設定します。
    /// </summary>
    /// <param name="limit">件数</param>
    /// <returns>このオプションオブジェクト自身</returns>
    public SearchVoiceModelsOptions SetLimit(int limit)
    {
        if (limit < 1 || limit > 30)
        {
            throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be between 1 and 30.");
        }
        Limit = limit;
        return this;
    }
}