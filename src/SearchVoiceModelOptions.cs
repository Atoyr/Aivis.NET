namespace Aivis;

public class SearchVoiceModelsOptions
{
    /// <summary>
    /// 検索キーワード
    /// </summary>
    public string? Keyword { get; set; }

    private List<string> _tags = new();
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


    public string Sort { get; set; } = "download";

    public int Page { get; set; } = 1;

    public int Limit { get; set; } = 24;

    public SearchVoiceModelsOptions()
    {
    }

    public SearchVoiceModelsOptions SetKeyword(string? keyword)
    {
        Keyword = keyword?.Trim();
        return this;
    }

    public SearchVoiceModelsOptions SetTags(params string[] tags)
    {
        Tags = tags?.Select(t => t.Trim()).ToArray();
        return this;
    }

    public SearchVoiceModelsOptions SetCategories(params string[] categories)
    {
        Categories = categories?.Select(c => c.Trim()).ToArray();
        return this;
    }

    public SearchVoiceModelsOptions SetVoiceTimbres(params string[] voiceTimbres)
    {
        VoiceTimbres = voiceTimbres?.Select(t => t.Trim()).ToArray();
        return this;
    }

    public SearchVoiceModelsOptions SetLicenceTypes(params string[] licenceTypes)
    {
        LicenceTypes = licenceTypes?.Select(t => t.Trim()).ToArray();
        return this;
    }

    public SearchVoiceModelsOptions SetSort(string sort)
    {
        Sort = sort.Trim();
        return this;
    }

    public SearchVoiceModelsOptions SetPage(int page)
    {
        if (page < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than or equal to 1.");
        }
        Page = page;
        return this;
    }

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