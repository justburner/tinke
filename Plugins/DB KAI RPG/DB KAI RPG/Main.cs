/*
 * Copyright (C) 2022  Justburner
 *
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <http://www.gnu.org/licenses/>. 
 *
 * Developer: Justburner
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ekona;
using Ekona.Images;

namespace DB_KAI_RPG
{
    public class Main : IGamePlugin
    {
        IPluginHost pluginHost;
        string gameCode;

        public void Initialize(IPluginHost pluginHost, string gameCode)
        {
            this.pluginHost = pluginHost;
            this.gameCode = gameCode;
        }

        public bool IsCompatible()
        {
            return (gameCode == "BRPE");
        }

        public Format Get_Format(sFile file, byte[] magic)
        {
            if (file.name.ToUpper().EndsWith(".CHR"))
                return Format.Cell;
            if (file.name.ToUpper().EndsWith(".IMG"))
                return Format.FullImage;

            return Format.Unknown;
        }

        public void Read(sFile file)
        {
            if (file.name.ToUpper().EndsWith(".CHR"))
            {
                CHR dCHR = new CHR(file.path, file.id, file.name);
                pluginHost.Set_Object(dCHR);
            }
            else if (file.name.ToUpper().EndsWith(".IMG"))
            {
                IMG dIMG = new IMG(file.path, file.id, file.name);
                pluginHost.Set_Object(dIMG);
            }
        }
        public Control Show_Info(sFile file)
        {
            Read(file);

            if (file.name.ToUpper().EndsWith(".CHR"))
                return new CHRControl(pluginHost, new CHR(file.path, file.id, file.name));
            else if (file.name.ToUpper().EndsWith(".IMG"))
                return new IMGControl(pluginHost, new IMG(file.path, file.id, file.name));

            return new Control();
        }

        // Non implemented
        public String Pack(ref sFolder unpacked, sFile file) { return null; }
        public sFolder Unpack(sFile file) { return new sFolder(); }
    }
}
