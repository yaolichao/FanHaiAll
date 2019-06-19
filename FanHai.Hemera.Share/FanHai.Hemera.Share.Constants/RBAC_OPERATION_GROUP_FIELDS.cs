using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FanHai.Hemera.Share.Constants
{
    public class RBAC_OPERATION_GROUP_FIELDS:TableFields
    {
      public const string FIELD_OPERATION_GROUP_KEY  ="OPERATION_GROUP_KEY";
      public const string FIELD_GROUP_NAME  ="GROUP_NAME";
      public const string FIELD_DESCRIPTIONS  ="DESCRIPTIONS";
      public const string FIELD_CREATOR  ="CREATOR";
      public const string FIELD_CREATE_TIME  ="CREATE_TIME";
      public const string FIELD_EDITOR  ="EDITOR";
      public const string FIELD_EDIT_TIME  ="EDIT_TIME";
      public const string FIELD_REMARK  ="REMARK";
      public const string FIELD_CREATE_TIMEZONE ="CREATE_TIMEZONE";
      public const string FIELD_EDIT_TIMEZONE  ="EDIT_TIMEZONE";

      #region table name define
      public const string DATABASE_TABLE_NAME = "RBAC_OPERATION_GROUP";       //TABLE NAME
      #endregion

      #region get TABLE_NAME
      /// <summary>
      /// override TABLE_NAME
      /// </summary>
      public override string TABLE_NAME
      {
          get
          {
              return DATABASE_TABLE_NAME;
          }
      }
      #endregion

      #region get FIELDS
      /// <summary>
      /// override fields
      /// </summary>
      public override Dictionary<string, FieldProperties> FIELDS
      {
          get
          {
              return _fields;
          }
      }
      #endregion

      #region define dictionary of containing column's fields
      //define _fields
      private static Dictionary<string, FieldProperties> _fields = new Dictionary<string, FieldProperties>
                                                              {
                                                                  {FIELD_OPERATION_GROUP_KEY , new FieldProperties() },       
                                                                  {FIELD_GROUP_NAME , new FieldProperties() },
                                                                  {FIELD_DESCRIPTIONS, new FieldProperties()},
                                                                  {FIELD_CREATOR , new FieldProperties() },  
                                                                  {FIELD_CREATE_TIME , new FieldProperties(typeof(DateTime),true,DateTime.Now)}, 
                                                                  {FIELD_CREATE_TIMEZONE , new FieldProperties() },  
                                                                  {FIELD_EDITOR , new FieldProperties() },
                                                                  {FIELD_EDIT_TIME , new FieldProperties(typeof(DateTime),true,DateTime.Now) }, 
                                                                  {FIELD_EDIT_TIMEZONE , new FieldProperties() }, 
                                                                  {FIELD_REMARK , new FieldProperties() }  
                                                                };
      #endregion
    } 
}
