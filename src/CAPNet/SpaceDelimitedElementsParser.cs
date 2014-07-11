﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CAPNet
{
    /// <summary>
    /// 
    /// </summary>
    public static class SpaceDelimitedElementsParser
    {
        private const int BETWEEN_ELEMENTS = 0;
        private const int IN_SPACE_CONTAINING_ELEMENTS = 1;
        private const int IN_ELEMENTS_WITH_NO_SPACE = 2;
    
        private static int state;
        private static string partialElement;
        private static int currentPosition;
        private static List<string> elements;
        private static string representation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Multiple space-delimited elements MAY be included.
        /// Elements including whitespace MUST be enclosed in double-quotes.</param>
        /// <returns></returns>
        public static IEnumerable<string> GetElements(this string value)
        {
            representation = value;
            elements = new List<string>();
            partialElement = "";
            state = BETWEEN_ELEMENTS;

            for (currentPosition = 0; currentPosition < representation.Length; currentPosition++)
            {
                char currentChar = representation[currentPosition];
                switch (state)
                {
                    case BETWEEN_ELEMENTS:
                        BetweenElementsState(currentChar);
                        break;

                    case IN_ELEMENTS_WITH_NO_SPACE:
                        InAddressWithNoSpaceState(currentChar);
                        break;

                    case IN_SPACE_CONTAINING_ELEMENTS:
                        InSpaceContainingAddressState(currentChar);
                        break;
                }
            }

            return elements;
        }

        private static void InSpaceContainingAddressState(char currentChar)
        {
            if (currentChar.IsElementCharacterOrSpace())
            {
                partialElement += currentChar;
                if (currentPosition == representation.Length - 1)
                {
                    elements.Add(partialElement);
                }
            }
            else if (currentChar.IsQuote())
            {
                elements.Add(partialElement);
                partialElement = "";
                state = BETWEEN_ELEMENTS;
            }
        }

        private static void InAddressWithNoSpaceState(char currentChar)
        {
            if (currentChar.IsElementCharacter())
            {
                partialElement += currentChar;
                if (currentPosition == representation.Length - 1)
                {
                    elements.Add(partialElement);
                }
            }
            else if (currentChar.IsSpace())
            {
                elements.Add(partialElement);
                partialElement = "";
                state = BETWEEN_ELEMENTS;
            }
            
        }

        private static void BetweenElementsState(char currentChar)
        {
            if (currentChar.IsQuote())
            {
                state = IN_SPACE_CONTAINING_ELEMENTS;
            }
            else if (currentChar.IsElementCharacter())
            {
                state = IN_ELEMENTS_WITH_NO_SPACE;
                partialElement += currentChar;
                if (currentPosition == representation.Length - 1)
                {
                    elements.Add(partialElement);
                }
            }
        }

        private static bool IsElementCharacter(this char tested)
        {
            return !tested.IsQuote() && !tested.IsSpace();
        }

        private static bool IsSpace(this char tested)
        {
            return tested == ' ';
        }

        private static bool IsQuote(this char tested)
        {
            return tested == '"';
        }

        private static bool IsElementCharacterOrSpace(this char tested)
        {
            return tested.IsElementCharacter() || tested.IsSpace();
        }
    }

}
