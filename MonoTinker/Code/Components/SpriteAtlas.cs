
namespace MonoTinker.Code.Components
{
    using System.Text.RegularExpressions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;

    using Elements;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class SpriteAtlas : IOrderedDictionary, IDictionary<string, Sprite>
    {
        private const int DefaultSize = 0;
        private readonly int initialCapacity;
        private object _syncRoot;
        private Dictionary<string, Sprite> _dictionary;
        private List<KeyValuePair<string, Sprite>> _list;
        private List<Texture2D> sheets;
        private Vector2 frameSize;
        private bool isUniformSize;

        public SpriteAtlas(int size)
        {
            sheets = new List<Texture2D>();
            isUniformSize = true;
            initialCapacity = size;
        }

        public SpriteAtlas() : this(DefaultSize)
        {
            
        }

        /// <summary>
        /// Add Sprites to atlas from texture cut to a given size
        /// </summary>
        /// <param name="texture">Texture to slice</param>
        /// <param name="framesize">Frame size</param>
        /// <param name="basename">Base name for the slices</param>
        /// <param name="missing">Missing frames if frames not even</param>
        /// <returns>String array with the given names</returns>
        public string[] PopulateFromSpriteSheet(Texture2D texture,Vector2 framesize, string basename = "", int missing = 0)
        {
            List<string> names = new List<string>();
            if (frameSize == Vector2.Zero)
            {
                frameSize = framesize;
            }
            else if (frameSize != framesize)
            {
                isUniformSize = false;
            }
            sheets.Add(texture);
            Point size = framesize.ToPoint();
            int perRow = texture.Width / size.X;
            int perCol = texture.Height / size.Y;
            int total = (perCol * perRow) - missing;
            int index = 0;
            for (int y = 0; y < perCol; y++)
            {
                for (int x = 0; x < perRow; x++)
                {
                    Add(basename + index, new Sprite(texture, new Rectangle(x * size.X, y * size.Y, size.X, size.Y)));
                    names.Add(basename+index);
                    index++;
                    if (index >= total)
                    {
                        break;
                    }
                }
            }
            return names.ToArray();
        }


        /// <summary>
        /// Populate SpriteAtlas from MonoGame TexturePacker SpriteSheet and Txt file
        /// </summary>
        /// <param name="content">Content manager to load with</param>
        /// <param name="filename">File name</param>
        public void PopulateFromSpriteSheet(ContentManager content, string filename)
        {
            Texture2D texture2D = content.Load<Texture2D>(filename);
            sheets.Add(texture2D);

            string file = Path.Combine(content.RootDirectory, Path.ChangeExtension(filename, "txt"));

            string[] entries =
                File.ReadAllLines(file).Where(s => !string.IsNullOrWhiteSpace(s) && !s.StartsWith("#")).ToArray();
            foreach (var entry in entries.Select(s => s.Split(';')))
            {
                string name = entry[0];
                bool isRotated = int.Parse(entry[1]) == 1;
                Rectangle source = new Rectangle()
                {
                    X = int.Parse(entry[2]),
                    Y = int.Parse(entry[3]),
                    Width = int.Parse(entry[4]),
                    Height = int.Parse(entry[5])
                };
                Vector2 size = new Vector2()
                {
                    X = int.Parse(entry[6]),
                    Y = int.Parse(entry[7])
                };

                Vector2 center = new Vector2()
                {
                    X = float.Parse(entry[8]),
                    Y = float.Parse(entry[9])
                };

                Add(name,new Sprite(texture2D,source,center,size,isRotated));

            }
        }

        /// <summary>
        /// Alternative way to populate from TexturePacker LibGX files
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filename"></param>
        public void PopulateFromSpriteSheetAlt(ContentManager content, string filename)
        {
            Texture2D texture2D = content.Load<Texture2D>(filename);
            sheets.Add(texture2D);
            string file = Path.Combine(content.RootDirectory, Path.ChangeExtension(filename, "txt"));
            string text = File.ReadAllText(file);

            Regex rgx = new Regex(@"(?<name>[^\n]*)\s*(?:rotate:\s[^\n]*)\s*\w*:\s*(?<x>\d*),\s(?<y>\d*)\s*\w*:\s*(?<sx>\d*),\s(?<sy>\d*)");
            MatchCollection mathes = rgx.Matches(text);

            foreach (Match match in mathes)
            {
                string name = match.Groups["name"].Value;
                int x = int.Parse(match.Groups["x"].Value);
                int y = int.Parse(match.Groups["y"].Value);
                int width = int.Parse(match.Groups["sx"].Value);
                int height = int.Parse(match.Groups["sy"].Value);
                Rectangle source = new Rectangle(x,y,width,height);
                Sprite toAdd = new Sprite(texture2D,source);
                toAdd.Origin = Origin.TopLeft;
                this.Add(name,toAdd);
            }
        }

 /*      
  SliderMiddle
  rotate: false
  xy: 2, 2
  size: 10, 5
  orig: 10, 5
  offset: 0, 0
  index: -1*/

        #region Add Methods

        public void Add(string key, Sprite value)
        {
            Dictionary.Add(key, value);
            List.Add(new KeyValuePair<string, Sprite>(key, value));

        }

        public void Add(object key, object value)
        {
            var k = key as string;
            var v = value as Sprite;
            if (k == null)
            {
                throw new ArgumentNullException(nameof(k), "The key must contain a value");
            }
            if (v == null)
            {
                throw new ArgumentNullException(nameof(v), "The key must contain a value");
            }
            Add(k, v);

        }

        public void Add(KeyValuePair<string, Sprite> item)
        {
            Dictionary.Add(item.Key, item.Value);
            List.Add(item);
        }

        #endregion

        #region Remove Methods

        public bool Remove(KeyValuePair<string, Sprite> item)
        {
            return Remove(item.Key);
        }

        public void Remove(object key)
        {
            var stringkey = key as string;
            if (stringkey != null)
            {
                Remove(stringkey);
                return;
            }
            if (key is int)
            {
                RemoveAt((int)key);
                return;
            }
            throw new ArgumentException("invalid object");
        }

        public bool Remove(string key)
        {
            if (_dictionary.ContainsKey(key))
            {
                _dictionary.Remove(key);
                int index = IndexOfKey(key);
                if (index >= 0)
                {
                    _list.RemoveAt(index);
                }
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if (index >= Count || index < 0)
            {
                throw new IndexOutOfRangeException("SpriteAtlas Index out of range");
            }
            string key = _list[index].Key;
            Remove(key);
        }

        #endregion

        #region Contains Methods

        public bool Contains(object key)
        {
            string k = key as string;
            if (k != null)
            {
                return ContainsKey(k);
            }
            return false;
        }

        public bool Contains(KeyValuePair<string, Sprite> item)
        {
            if (Dictionary.ContainsKey(item.Key))
            {
                return true;
            }
            return false;
        }

        public bool ContainsKey(string key)
        {
            return Dictionary.ContainsKey(key);
        }


        #endregion

        #region ICollections

        public ICollection Keys
        {
            get { return Dictionary.Keys; }
        }

        ICollection<Sprite> IDictionary<string, Sprite>.Values
        {
            get { return Dictionary.Values; }
        }

        ICollection<string> IDictionary<string, Sprite>.Keys
        {
            get { return Dictionary.Keys; }
        }

        public ICollection Values
        {
            get { return Dictionary.Values; }
        }

        #endregion

        #region Getters - Setters

        private Dictionary<string, Sprite> Dictionary
        {
            get {
                if (_dictionary == null)
                {
                    _dictionary = new Dictionary<string, Sprite>(initialCapacity);
                }
                return _dictionary;
            }
        }

        private List<KeyValuePair<string, Sprite>> List
        {
            get
            {
                if (_list == null)
                {
                    _list = new List<KeyValuePair<string, Sprite>>(initialCapacity);
                }
                return _list;
            }
        } 

        object IOrderedDictionary.this[int index]
        {
            get
            {
                if (index >= Count || index < 0)
                {
                    throw new IndexOutOfRangeException("SpriteAtlas index out of bounds");
                }
                return this.List[index].Value;
            }
            set
            {
                if (index >= Count || index < 0)
                {
                    throw new IndexOutOfRangeException("SpriteAtlas index out of bounds");
                }
                string key = List[index].Key;
                this[key] = (Sprite) value;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                var keystring = key as string;
                if (keystring != null)
                {
                    return this[keystring];
                }
                if (key is int)
                {
                    return this[(int)key];
                }
                throw new InvalidOperationException("Invalid key");
            }
            set
            {
                var sprite = value as Sprite;
                if (sprite != null)
                {
                    var s = key as string;
                    if (s != null)
                    {
                        this[s] = sprite;
                    }
                    if (key is int)
                    {
                        string newKey = List[(int) key].Key;
                        this.List[(int) key] = new KeyValuePair<string, Sprite>(newKey, sprite);
                    }
                }
            }
        }

        public Sprite this[string key]
        {
            get
            {
               return this.Dictionary[key];
            }
            set
            {
                this.Dictionary[key] = value;
                int index = IndexOfKey(key);
                if (index >= 0)
                {
                    List[index] = new KeyValuePair<string, Sprite>(key, value);
                }
            }
        }

        public Sprite this[int index]
        {
            get
            {
                if (index >= Count || index < 0)
                {
                    throw new IndexOutOfRangeException("SpriteAtlas index out of bounds");
                }
                return this.List[index].Value;
            }
            set
            {
                if (index >= Count || index < 0)
                {
                    throw new IndexOutOfRangeException("SpriteAtlas index out of bounds");
                }
                this[List[index].Key] = value;
            }
        }

        public bool TryGetValue(string key, out Sprite value)
        {
            try
            {
                value = Dictionary[key];
            }
            catch
            {
                value = null;
                return false;
            }
            return true;
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public int Count
        {
            get { return this.List.Count; }
        }

        public object SyncRoot
        {
            get
            {
                if (_syncRoot == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref _syncRoot, new object(), null);
                }
                return this._syncRoot;
            }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        #endregion

        #region Enumerators

        public IEnumerator<KeyValuePair<string, Sprite>> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }

        IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }


        #endregion

        #region Methods

        public void Insert(int index, object key, object value)
        {
            if (index >= Count || index < 0)
            {
                throw new IndexOutOfRangeException("SpriteAtlas invalid insert index");
            }

            string k = key as string;
            Sprite s = value as Sprite;
            if (k == null || s == null)
            {
                throw new ArgumentNullException(nameof(key) + " / " + nameof(value), "Arguments cant be null");
            }

            List.Insert(index, new KeyValuePair<string, Sprite>(k, s));
            Dictionary.Add(k, s);
        }

        public int IndexOfKey(string key)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].Key == key) return i;
            }
            return -1;
        }

        public void Clear()
        {
            Dictionary.Clear();
            List.Clear();
        }

        public void CopyTo(KeyValuePair<string, Sprite>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, Sprite>>)Dictionary).CopyTo(array, arrayIndex);
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)List).CopyTo(array, index);
        }

        #endregion


    }
}
