// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Net.Http.Headers
{
    public class EntityTagHeaderValue : ICloneable
    {
        private readonly string _tag;
        private readonly bool _isWeak;

        public string Tag
        {
            get { return _tag; }
        }

        public bool IsWeak
        {
            get { return _isWeak; }
        }

        public static EntityTagHeaderValue Any { get; } = new EntityTagHeaderValue();

        private EntityTagHeaderValue()
        {
            _tag = "*";
        }

        public EntityTagHeaderValue(string tag)
            : this(tag, false)
        {
        }

        public EntityTagHeaderValue(string tag, bool isWeak)
        {
            if (string.IsNullOrEmpty(tag))
            {
                throw new ArgumentException(SR.net_http_argument_empty_string, nameof(tag));
            }
            int length;
            if ((HttpRuleParser.GetQuotedStringLength(tag, 0, out length) != HttpParseResult.Parsed) ||
                (length != tag.Length))
            {
                // Note that we don't allow 'W/' prefixes for weak ETags in the 'tag' parameter. If the user wants to
                // add a weak ETag, they can set 'isWeak' to true.
                throw new FormatException(SR.net_http_headers_invalid_etag_name);
            }

            _tag = tag;
            _isWeak = isWeak;
        }

        private EntityTagHeaderValue(EntityTagHeaderValue source)
        {
            Debug.Assert(source != null);

            _tag = source._tag;
            _isWeak = source._isWeak;
        }

        public override string ToString()
        {
            if (_isWeak)
            {
                return "W/" + _tag;
            }
            return _tag;
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            EntityTagHeaderValue? other = obj as EntityTagHeaderValue;

            if (other == null)
            {
                return false;
            }

            // Since the tag is a quoted-string we treat it case-sensitive.
            return ((_isWeak == other._isWeak) && string.Equals(_tag, other._tag, StringComparison.Ordinal));
        }

        public override int GetHashCode()
        {
            // Since the tag is a quoted-string we treat it case-sensitive.
            return _tag.GetHashCode() ^ _isWeak.GetHashCode();
        }

        public static EntityTagHeaderValue Parse(string? input)
        {
            int index = 0;
            return (EntityTagHeaderValue)GenericHeaderParser.SingleValueEntityTagParser.ParseValue(
                input, null, ref index);
        }

        public static bool TryParse([NotNullWhen(true)] string? input, [NotNullWhen(true)] out EntityTagHeaderValue? parsedValue)
        {
            int index = 0;
            parsedValue = null;

            if (GenericHeaderParser.SingleValueEntityTagParser.TryParseValue(input, null, ref index, out object? output))
            {
                parsedValue = (EntityTagHeaderValue)output!;
                return true;
            }
            return false;
        }

        internal static int GetEntityTagLength(string? input, int startIndex, out EntityTagHeaderValue? parsedValue)
        {
            Debug.Assert(startIndex >= 0);

            parsedValue = null;

            if (string.IsNullOrEmpty(input) || (startIndex >= input.Length))
            {
                return 0;
            }

            // Caller must remove leading whitespace. If not, we'll return 0.
            bool isWeak = false;
            int current = startIndex;

            char firstChar = input[startIndex];
            if (firstChar == '*')
            {
                // We have '*' value, indicating "any" ETag.
                parsedValue = Any;
                current++;
            }
            else
            {
                // The RFC defines 'W/' as prefix, but we'll be flexible and also accept lower-case 'w'.
                if ((firstChar == 'W') || (firstChar == 'w'))
                {
                    current++;
                    // We need at least 3 more chars: the '/' character followed by two quotes.
                    if ((current + 2 >= input.Length) || (input[current] != '/'))
                    {
                        return 0;
                    }
                    isWeak = true;
                    current++; // we have a weak-entity tag.
                    current += HttpRuleParser.GetWhitespaceLength(input, current);
                }

                int tagStartIndex = current;
                int tagLength;
                if (HttpRuleParser.GetQuotedStringLength(input, current, out tagLength) != HttpParseResult.Parsed)
                {
                    return 0;
                }

                if (tagLength == input.Length)
                {
                    // Most of the time we'll have strong ETags without leading/trailing whitespace.
                    Debug.Assert(startIndex == 0);
                    Debug.Assert(!isWeak);
                    parsedValue = new EntityTagHeaderValue(input);
                }
                else
                {
                    parsedValue = new EntityTagHeaderValue(input.Substring(tagStartIndex, tagLength), isWeak);
                }

                current += tagLength;
            }
            current += HttpRuleParser.GetWhitespaceLength(input, current);

            return current - startIndex;
        }

        object ICloneable.Clone() => ReferenceEquals(this, Any) ? Any : new EntityTagHeaderValue(this);
    }
}
