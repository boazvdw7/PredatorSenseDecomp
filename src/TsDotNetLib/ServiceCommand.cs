using System;

namespace TsDotNetLib
{
	// Token: 0x02000011 RID: 17
	public enum ServiceCommand
	{
		// Token: 0x04000084 RID: 132
		kSvcTest_EchoArguments,
		// Token: 0x04000085 RID: 133
		kSvcCmdRegCreateKey,
		// Token: 0x04000086 RID: 134
		kSvcCmdRegDeleteKey,
		// Token: 0x04000087 RID: 135
		kSvcCmdRegCreateValue,
		// Token: 0x04000088 RID: 136
		kSvcCmdRegDeleteValue,
		// Token: 0x04000089 RID: 137
		kSvcCmdRegSetValue,
		// Token: 0x0400008A RID: 138
		kSvcCmdRegGetValue,
		// Token: 0x0400008B RID: 139
		kSvcCmdCreateProcessAsUser,
		// Token: 0x0400008C RID: 140
		kSvcCmdCreateProcessOnLogon,
		// Token: 0x0400008D RID: 141
		kSvcCmdGamingPorfileWMISetFunction,
		// Token: 0x0400008E RID: 142
		kSvcCmdGamingPorfileWMIGetFunction,
		// Token: 0x0400008F RID: 143
		kSvcCmdGamingLEDGroupColorSetFunction,
		// Token: 0x04000090 RID: 144
		kSvcCmdGamingLEDGroupColorGetFunction,
		// Token: 0x04000091 RID: 145
		kSvcCmdGetGamingSysinfoFunction,
		// Token: 0x04000092 RID: 146
		kSvcCmdGetGPUUsageLoading,
		// Token: 0x04000093 RID: 147
		kSvcCmdGamingFanGroupBehaviorSetFunction,
		// Token: 0x04000094 RID: 148
		kSvcCmdGamingFanGroupSpeedSetFunction,
		// Token: 0x04000095 RID: 149
		kSvcCmdWMISetFunction,
		// Token: 0x04000096 RID: 150
		kSvcCmdWMISetLogoLED,
		// Token: 0x04000097 RID: 151
		kSvcCmdWMISetLogoBehavior,
		// Token: 0x04000098 RID: 152
		kSvcCmdWMIGetFunction,
		// Token: 0x04000099 RID: 153
		kSvcCmdNvidiaOCGetFunction,
		// Token: 0x0400009A RID: 154
		kSvcCmdIntelOCGetFunction,
		// Token: 0x0400009B RID: 155
		kSvcCmdNvidiaOCSetFunction,
		// Token: 0x0400009C RID: 156
		kSvcCmdIntelOCSetFunction,
		// Token: 0x0400009D RID: 157
		kSvcCmdNvidiaSLISetFunction,
		// Token: 0x0400009E RID: 158
		kSvcCmdNvidiaGetCoprocStatusFunction
	}
}
