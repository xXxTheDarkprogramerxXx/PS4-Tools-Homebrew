using System;
using System.Runtime.InteropServices;

namespace Sony
{
	namespace NP
	{
		/// <summary>
		/// Contains the profile classed and structures common to the Friends and UserProfile systems.
		/// </summary>
		public class Profiles
		{
			/// <summary>
			/// This class represents a Real Name of a user.
			/// </summary>
			public class RealName
			{
				/// <summary>
				/// The maximum size of the first name
				/// </summary>
				public const int MAX_SIZE_FIRST_NAME = 16;
				/// <summary>
				/// The maximum size of the middle name
				/// </summary>
				public const int MAX_SIZE_MIDDLE_NAME = 16;
				/// <summary>
				/// The maximum size of the last name
				/// </summary>
				public const int MAX_SIZE_LAST_NAME = 16;

				internal string firstName = "";
				internal string middleName = "";
				internal string lastName = "";		

				/// <summary>
				/// The first name of the user. It can contain spaces or hyphens
				/// </summary>
				public string FirstName { get { return firstName; } }

				/// <summary>
				/// The middle name of the user. It can contain spaces or hyphens
				/// </summary>
				public string MiddleName { get { return middleName; } }

				/// <summary>
				/// The last name of the user. It can contain spaces or hyphens
				/// </summary>
				public string LastName { get { return lastName; } }

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.RealNameBegin);

					buffer.ReadString(ref firstName);
					buffer.ReadString(ref middleName);
					buffer.ReadString(ref lastName);

					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.RealNameEnd);
				}
			}

			/// <summary>
			/// Representation of the profile of a user.
			/// </summary>
			public class Profile
			{
				#region DLL Imports

				#endregion

				/// <summary>
				/// Enum representing the relationship between the calling user and the target user.
				/// </summary>
				public enum RelationTypes
				{
					/// <summary>Relation type was not set</summary>
					notSet = 0,
					/// <summary>The profile is the calling user one</summary>
					me,
					/// <summary>The profile is from a friend of the calling user</summary>
					friends,
					/// <summary>The profile is from someone who wants to be a friend of the calling user</summary>
					requestingFriend,
					/// <summary>The profile is from someone whom the calling user wants to be a friend of</summary>
					requestedFriend,
					/// <summary>The profile is from a user the calling user has blocked</summary>
					blocked,
					/// <summary>The profile is from a friend of a friend of the calling user</summary>
					friendOfFriends,
					/// <summary>The profile is for someone that has no relationship with the calling user</summary>
					noRelationship		
				};

				/// <summary>
				/// Enum representing the personal detailed information provided in the user profile of the target user.
				/// </summary>
				public enum PersonalDetailsTypes
				{
					/// <summary>The user has not set any personal details or they can't be obtained by the calling user</summary>
					none = 0,
					/// <summary>The user has set the real name</summary>
					realName = 1,
					/// <summary>The user has a verified account with a display name set</summary>
					verifiedAccountDisplayName = 2
				};

				#region Properties

				/// <summary>
				/// The maximum size of <see cref="AboutMe"/>
				/// </summary>
				public const int MAX_SIZE_ABOUT_ME = 140;
				/// <summary>
				/// The maximum size of the <see cref="AvatarUrl"/>
				/// </summary>
				public const int MAX_SIZE_AVATAR_URL = 128;

				/// <summary>
				/// The maximum number of languages that can be set in the profile
				/// </summary>
				public const int MAX_NUM_LANGUAGES_USED = 3;
				
				/// <summary>
				/// The maximum size of the display name field for <see cref="VerifiedAccountDisplayName"/>
				/// </summary>
				public const int MAX_SIZE_VERIFIED_ACCOUNT_DISPLAY_NAME = 32;
				
				/// <summary>
				/// The maximum size of the <see cref="ProfilePictureUrl"/>
				/// </summary>
				public const int MAX_SIZE_PROFILE_PICTURE_URL = 256;			

				internal Core.OnlineUser onlineUser = new Core.OnlineUser();
				internal RelationTypes relationType;
				internal Core.LanguageCode[] languagesUsed = new Core.LanguageCode[MAX_NUM_LANGUAGES_USED];
				internal Core.CountryCode country = new Core.CountryCode();
				internal PersonalDetailsTypes personalDetailsType;
				internal RealName realName;

				internal string verifiedAccountDisplayName;

				internal string aboutMe = "";
				internal string avatarUrl = "";
				internal string profilePictureUrl = "";
				internal bool isVerifiedAccount;	

				/// <summary>
				/// The user whose profile is retrieved
				/// </summary>
				public Core.OnlineUser OnlineUser { get { return onlineUser; } }

				/// <summary>
				/// The relation between the profile user and the calling user
				/// </summary>
				public RelationTypes RelationType { get { return relationType; } }

				/// <summary>
				/// The country code in ISO 3166-1 format (two-letter system)
				/// </summary>
				public Core.CountryCode Country { get { return country; } }

				/// <summary>
				/// An array of the languages used starting with the primary language onwards. A five digit format (countryCode-language) is used.
				/// </summary>
				public Core.LanguageCode[] LanguagesUsed { get { return languagesUsed; } }

				/// <summary>
				/// The kind of personal information the user has set on the account
				/// </summary>
				/// <remarks>
				/// The personal details identifying the user who owns the account. See <see cref="RealName"/> and <see cref="VerifiedAccountDisplayName"/> for more information of
				/// the details provided.
				/// </remarks>
				public PersonalDetailsTypes PersonalDetailsType { get { return personalDetailsType; } }

				/// <summary>
				/// The first name, middle name and last name of the user
				/// </summary>
				/// <remarks>
				/// This should only be called if <see cref="PersonalDetailsType"/> is set to <see cref="PersonalDetailsTypes.realName"/>
				/// </remarks>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="PersonalDetailsType"/> isn't set to <see cref="PersonalDetailsTypes.realName"/></exception>
				public RealName RealName
				{
					get
					{
						if (personalDetailsType != PersonalDetailsTypes.realName)
						{
							throw new NpToolkitException("Can't access RealName unless PersonalDetailsType is PersonalDetailsType.realName");
						}

						return realName;
					}
				}

				/// <summary>
				/// Verified Accounts can set a display name. If they do, then real name can't be provided as per TRC
				/// </summary>
				/// <remarks>
				/// This should only be called if <see cref="PersonalDetailsType"/> is set to <see cref="PersonalDetailsTypes.verifiedAccountDisplayName"/>
				/// </remarks>
				/// <exception cref="NpToolkitException">Will throw an exception if <see cref="PersonalDetailsType"/> isn't set to <see cref="PersonalDetailsTypes.verifiedAccountDisplayName"/></exception>
				public string VerifiedAccountDisplayName
				{
					get
					{
						if (personalDetailsType != PersonalDetailsTypes.verifiedAccountDisplayName)
						{
							throw new NpToolkitException("Can't access VerifiedAccountDisplayName unless PersonalDetailsType is PersonalDetailsType.verifiedAccountDisplayName");
						}

						return verifiedAccountDisplayName;
					}
				}

				/// <summary>
				/// If this profile is from a Verified Account
				/// </summary>
				public bool IsVerifiedAccount { get { return isVerifiedAccount; } }

				/// <summary>
				/// Information set by the user on its profile
				/// </summary>
				public string AboutMe { get { return aboutMe; } }

				/// <summary>
				/// The avatar picture URL of the profile
				/// </summary>
				public string AvatarUrl { get { return avatarUrl; } }

				/// <summary>
				/// The profile picture URL of the user. Usually a real picture (when linked to Facebook, etc.)
				/// </summary>
				public string ProfilePictureUrl { get { return profilePictureUrl; } }

				#endregion

				/// <summary>
				/// Initializes a new instance of the <see cref="Profile"/> class.
				/// </summary>
				public Profile()
				{
					for (int i = 0; i < MAX_NUM_LANGUAGES_USED; i++)
					{
						languagesUsed[i] = new Core.LanguageCode();
					}
				}

				// Read data from PRX marshaled buffer
				internal void Read(MemoryBuffer buffer)
				{
					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.ProfileBegin);

					onlineUser.Read(buffer);

					relationType = (RelationTypes)buffer.ReadUInt32();

					UInt32 languageArraySize = buffer.ReadUInt32();

					if (languageArraySize != MAX_NUM_LANGUAGES_USED)
					{
						throw new NpToolkitException("Unexpected language array size in Profile. Should be " + MAX_NUM_LANGUAGES_USED);
					}

					for (int i = 0; i < MAX_NUM_LANGUAGES_USED; i++)
					{
						languagesUsed[i].Read(buffer);
					}

					country.Read(buffer);

					personalDetailsType = (PersonalDetailsTypes)buffer.ReadUInt32();

					if (personalDetailsType == PersonalDetailsTypes.realName)
					{
						realName = new RealName();
						realName.Read(buffer);
					}
					else if (personalDetailsType == PersonalDetailsTypes.verifiedAccountDisplayName)
					{
						verifiedAccountDisplayName = "";
						buffer.ReadString(ref verifiedAccountDisplayName);
					}

					buffer.ReadString(ref aboutMe);

					buffer.ReadString(ref avatarUrl);

					buffer.ReadString(ref profilePictureUrl);

					isVerifiedAccount = buffer.ReadBool();

					buffer.CheckMarker(MemoryBuffer.BufferIntegrityChecks.ProfileEnd);
				}

				/// <summary>
				/// Generate an abbreviated string containing the profile data.
				/// </summary>
				/// <returns>The profile data</returns>
				public override string ToString()
				{
					string output = "";

					output += string.Format("{0} : Relation ({1}) CC ({2}) PT ({3}) Lang1 ({4})\n", OnlineUser.ToString(), RelationType, Country.ToString(), PersonalDetailsType, languagesUsed[0].ToString());

					if (PersonalDetailsType == PersonalDetailsTypes.realName)
					{
						output += string.Format(" RN ({0} {1} {2})\n", RealName.FirstName, RealName.MiddleName, RealName.LastName);
					}
					else if (PersonalDetailsType == PersonalDetailsTypes.verifiedAccountDisplayName)
					{
						output += string.Format(" VDN ({0})\n", VerifiedAccountDisplayName);
					}

					//output += string.Format(" About Me ({0})\n", AboutMe);
					//output += string.Format(" Avatar Url ({0})\n", AvatarUrl);
					//output += string.Format(" Picture Url ({0})\n", ProfilePictureUrl);
					output += string.Format(" Verified Account ({0})", IsVerifiedAccount);

					return output;
				}
			}
		}
	}
}
