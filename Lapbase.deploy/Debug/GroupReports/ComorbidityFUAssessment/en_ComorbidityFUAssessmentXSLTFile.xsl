<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">

<xsl:template match="/">
    <html>
    <head>
      <link href="../../CssFiles/cssclass.css" type="text/css" rel="stylesheet"/>
      <META http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    </head>
    <body>
      <table width ="100%" class = "Label">
        <tr>
          <td style = "width:50%; font-size : large; font-weight:bold">
            <xsl:value-of select="dsSchema/tblReport/PatientName"/>
          </td>
          <td style = "width:50%; font-size : larger; font-weight:bold">BASELINE COMORBIDITIES</td>
        </tr>
        <tr>
          <td colspan = "2">
            <a style= "font-size:medium; font-weight:bold;">AGE : </a>
            <xsl:value-of select="dsSchema/tblReport/AGE"/>&#160;&#160;
            <a style= "font-size:medium; font-weight:bold;">Address : </a>
            <xsl:value-of select="dsSchema/tblReport/Address"/>
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <table width = "100%" cellspacing="0" border="1px" >
              <tr style="Height:30px">
                <td colspan = "3" style= "border-bottom-width:0;font-size:medium; font-weight:bold; Color:BROWN">Surgery</td>
              </tr>
              <tr style="Height:30px">
                <td style ="border-top-width:0;border-bottom-width:0;border-right-width:0;width:30%">
                  <xsl:value-of select="dsSchema/tblReport/SurgeryType_Desc"/>
                </td>
                <td style ="border-top-width:0;border-bottom-width:0;border-left-width:0;border-right-width:0;width:30%">
                  <xsl:value-of select="dsSchema/tblReport/Approach"/>
                </td>
                <td style ="border-top-width:0;border-bottom-width:0;border-left-width:0;">value-of group</td>
              </tr>
              <tr  style="Height:30px">
                <td  style ="border-top-width:0;border-right-width:0;">
                  <a>Date : </a>
                  <xsl:value-of select="dsSchema/tblReport/strLapBandDate"/>
                </td>
                <td style ="border-top-width:0;border-left-width:0;border-right-width:0;">
                  <xsl:value-of select="dsSchema/tblReport/Category_Desc"/>
                </td>
                <td  style ="border-top-width:0;border-left-width:0;">
                  <xsl:value-of select="dsSchema/tblReport/DcctorID"/>
                </td>
              </tr>
            </table>
          </td>
        </tr>

        <tr>
          <td colspan="2">
            <br/>
          </td>
        </tr>
        
        <tr>
          <td colspan = "2">
            <table width = "100%" cellspacing="0" border="1px" >
              <tr style="Height:30px">
                <td colspan = "3" style= "border-bottom-width:0;font-size:medium; font-weight:bold; Color:BROWN">Initial Assessment</td>
              </tr>
              <tr style="Height:30px">
                <td  style ="border-top-width:0;border-bottom-width:0;border-right-width:0;">
                  <a>Height : </a><xsl:value-of select="dsSchema/tblReport/Height"/>&#160;&#160;
                  <xsl:value-of select="dsSchema/tblSysConfig/TxtHT"/>
                </td>
                <td style ="border-top-width:0;border-bottom-width:0;border-left-width:0;border-right-width:0;">
                  <a>BMI : </a>
                  <xsl:value-of select="dsSchema/tblReport/InitBMI"/>
                </td>
                <td  style ="border-top-width:0;border-bottom-width:0;border-left-width:0;">
                  <a>Target Weight : </a><xsl:value-of select="dsSchema/tblReport/TargetWeight"/>&#160;&#160;<xsl:value-of select="dsSchema/tblSysConfig/TxtWT"/>
                </td>
              </tr>

              <tr  style="Height:30px">
                <td  style ="border-top-width:0;border-right-width:0;">
                  <a>Weight : </a><xsl:value-of select="dsSchema/tblReport/StartWeight"/>&#160;&#160;<xsl:value-of select="dsSchema/tblSysConfig/TxtWT"/>
                </td>
                <td style ="border-top-width:0;border-left-width:0;border-right-width:0;">
                  <a>Ideal Weight: </a><xsl:value-of select="dsSchema/tblReport/IdealWeight"/>&#160;&#160;<xsl:value-of select="dsSchema/tblSysConfig/TxtWT"/>
                </td>
                <td  style ="border-top-width:0;border-left-width:0;">
                  <a>Neck : </a><xsl:value-of select="dsSchema/tblReport/StartNeck"/>&#160;&#160;
                  <a>Waist : </a><xsl:value-of select="dsSchema/tblReport/StartWaist"/>&#160;&#160;
                  <a>Hip : </a><xsl:value-of select="dsSchema/tblReport/StartHip"/>&#160;&#160;
                </td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <br/>
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <table width = "100%" >
              <tr style="height:40px">
                <td style= "width:75%;font-size:large; font-weight:bold;color:RED">Comorbidties</td>
                <td style= "width:15%;font-weight:bold">Resolved</td>
                <td style= "width:10%;font-weight:bold">date</td>
              </tr>
            </table>
          </td>
        </tr>
        
        <tr>
          <td colspan="2">
            <table width="100%" border ="1px" cellspacing="0">
              <tr style="height:40px">
                <td style ="width:75%;border-right-width:0;">
                  <img alt="." >
                      <xsl:if test= "dsSchema/tblReport/BaseSystolicBP != 0 or dsSchema/tblReport/BaseDiastolicBP != 0 ">
                          <xsl:attribute name="SRC">
                              <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif</xsl:attribute>
                      </xsl:if>
                  </img>
                  <a style= "font-size:medium; font-weight:bold; text-decoration:underline;">BLOOD PRESSURE</a>&#160;&#160;&#160;&#160;
                <xsl:value-of select="dsSchema/tblReport/BaseSystolicBP"/> / <xsl:value-of select="dsSchema/tblReport/BaseDiastolicBP"/>
              </td>
              <td style="width:15%;width:15%;border-right-width:0;border-left-width:0;">
                <xsl:value-of select="dsSchema/tblReport/HypertensionResolvedDate"/>
              </td>
              <td style="width:10%;border-left-width:0;">
                <xsl:value-of select="dsSchema/tblReport/HypertensionResolvedDate_Diff"/>
              </td>
            </tr>
          </table>
        </td>
      </tr>
      <tr >
        <td colspan="2">
          <table width="100%" border ="1px" cellspacing="0">
            <tr style="height:40px">
              <td rowspan = "2" style ="width:75%;border-right-width:0;" >
                <img alt="." >

                  <xsl:if test= "(dsSchema/tblReport/BaseTotalCholesterol != 0) or (dsSchema/tblReport/BaseHDLCholesterol != 0) or (dsSchema/tblReport/BaseTriglycerides != 0)">
                    <xsl:attribute name="SRC">
                      <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                    </xsl:attribute>
                  </xsl:if>

                </img>
                <a style= "font-size:medium; font-weight:bold; text-decoration:underline;">LIPIDS </a>&#160;&#160;
                  <a style ="font-weight:bold;">Chol. </a>
                  <xsl:value-of select="dsSchema/tblReport/BaseTotalCholesterol"/>&#160;&#160;
                  <a style ="font-weight:bold;">HDL. </a>
                  <xsl:value-of select="dsSchema/tblReport/BaseHDLCholesterol"/>&#160;&#160;
                  <a style ="font-weight:bold;">LDL. </a>
                  <xsl:value-of select="dsSchema/tblReport/BaseLDLCholesterol"/>&#160;&#160;
                  <a style ="font-weight:bold;">Trig. </a>
                  <xsl:value-of select="dsSchema/tblReport/BaseTriglycerides"/>&#160;&#160;
                </td>
                <td style="width:15%;border-right-width:0;border-left-width:0;">
                  <xsl:value-of select="dsSchema/tblReport/LipidsResolvedDate"/>
                </td>
                <td style="width:10%;border-left-width:0;">
                  <xsl:value-of select="dsSchema/tblReport/LipidsResolvedDate_Diff"/>
                </td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td colspan = "2">
            <table width="100%" border="1px" cellspacing="0">
              <tr style="height:40px">
                <td rowspan = "2" style ="width:75%;border-right-width:0;" >
                  <img alt="." >
                    
                          <xsl:if test= "(dsSchema/tblReport/BaseFBloodGlucose != 0)">
                            <xsl:attribute name="SRC">
                              <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                            </xsl:attribute>
                          </xsl:if>
                          
                  </img>
                  <a style= "font-size:medium; font-weight:bold; text-decoration:underline;">BLOOD SUGAR</a>&#160;&#160;
                  <xsl:value-of select="dsSchema/tblReport/BaseFBloodGlucose"/>
                </td>
                <td style="width:15%;border-right-width:0;border-left-width:0;">
                  <xsl:value-of select="dsSchema/tblReport/DiabetesResolvedDate"/>
                </td>
                <td style="width:10%;border-left-width:0;">
                  <xsl:value-of select="dsSchema/tblReport/DiabetesResolvedDate_Diff"/>
                </td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td colspan = "2">
            <table width="100%" border="1px" cellspacing="0">
              <tr style="height:40px">
                <td rowspan = "2" style ="width:75%;border-right-width:0;" >
                  <img alt="." >
                      <xsl:if test= "(dsSchema/tblReport/BaseAsthmaLevel != 0)">
                        <xsl:attribute name="SRC">
                          <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                        </xsl:attribute>
                      </xsl:if>
                  </img>
                  <a style= "font-size:medium; font-weight:bold; text-decoration:underline;">ASTHMA</a>&#160;&#160;
                  <xsl:value-of select="dsSchema/tblReport/strBaseAsthmaLevel"/>
                </td>
                <td style="width:15%;border-right-width:0;border-left-width:0;">
                  <xsl:value-of select="dsSchema/tblReport/AsthmaResolvedDate"/>
                </td>
                <td style="width:10%;border-left-width:0;">
                  <xsl:value-of select="dsSchema/tblReport/AsthmaResolvedDate_Diff"/>
                </td>
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td colspan = "2">
            <table width="100%" border="1px" cellspacing="0">
              <tr style="height:40px">
                <td rowspan = "2" style ="width:75%;border-right-width:0;" >
                  <img alt="." >
                      <xsl:if test= "(dsSchema/tblReport/BaseRefluxLevel != 0)">
                        <xsl:attribute name="SRC">
                          <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                        </xsl:attribute>
                      </xsl:if>
                  </img>
                  <a style= "font-size:medium; font-weight:bold; text-decoration:underline;">REFLUX</a>&#160;&#160;
                <xsl:value-of select="dsSchema/tblReport/strBaseRefluxLevel"/>
              </td>
              <td style="width:15%;border-right-width:0;border-left-width:0;">
                <xsl:value-of select="dsSchema/tblReport/RefluxResolvedDate"/>
              </td>
              <td style="width:10%;border-left-width:0;">
                <xsl:value-of select="dsSchema/tblReport/RefluxResolvedDate_Diff"/>
              </td>
            </tr>
          </table>
        </td>
      </tr>
      <tr>
        <td colspan = "2">
          <table width="100%" border="1px" cellspacing="0">
            <tr style="height:40px">
              <td style ="width:75%;border-right-width:0;" >
                <img alt="." >
                  <xsl:if test= "(dsSchema/tblReport/BaseSleepLevel != 0)">
                    <xsl:attribute name="SRC">
                      <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                    </xsl:attribute>
                  </xsl:if>
                </img>
                <a style= "font-size:medium; font-weight:bold; text-decoration:underline;">SLEEP</a>&#160;&#160;
                  <xsl:value-of select="dsSchema/tblReport/strBaseSleepLevel"/>
                </td>
                <td style="width:15%;border-right-width:0;border-left-width:0;">
                  <xsl:value-of select="dsSchema/tblReport/SleepResolvedDate"/>
                </td>
                <td style="border-left-width:0;">
                  <xsl:value-of select="dsSchema/tblReport/SleepResolvedDate_Diff"/>
                </td>
              </tr>
            </table>
          </td>
        </tr>

        <tr>
          <td colspan = "2">
            <table width="100%" border="1px" cellspacing="0">
              <tr>
                <td style="width:75%;border-right-width:0px;">
                  <img alt="." >
                      <xsl:if test= "(dsSchema/tblReport/BaseFertilityProblems = 'true')">
                        <xsl:attribute name="SRC">
                          <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                        </xsl:attribute>
                      </xsl:if>
                  </img>
                  &#160;<a style ="font-weight:bold">FERTILITY</a>
                </td>
                <td style="width:15%;border-right-width:0;border-left-width:0;">
                  <xsl:value-of select="dsSchema/tblReport/FertilityResolvedDate"/>
                </td>
                <td style="border-left-width:0;">
                  <xsl:value-of select="dsSchema/tblReport/FertilityResolvedDate_Diff"/>
                </td>
              </tr>
            </table>
          </td>
        </tr>

        <tr>
          <td colspan = "2">
            <table width="100%" border="1px" cellspacing="0">
              <tr>
                <td style="width:75%;border-right-width:0px;">
                    <img alt="." >
                        <xsl:if test= "(dsSchema/tblReport/BaseIncontinenceProblems = 'true')">
                          <xsl:attribute name="SRC">
                            <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                          </xsl:attribute>
                        </xsl:if>
                    </img>
                    &#160;<a style ="font-weight:bold">INCONTINENCE</a>
                  </td>
                  <td style="width:15%;border-right-width:0;border-left-width:0;">
                    <xsl:value-of select="dsSchema/tblReport/IncontinenceResolvedDate"/>
                  </td>
                  <td style="border-left-width:0;">
                    <xsl:value-of select="dsSchema/tblReport/IncontinenceResolvedDate_Diff"/>
                  </td>
                </tr>
              </table>
          </td>
        </tr>

        <tr>
          <td colspan = "2">
              <table width="100%" border="1px" cellspacing="0">
                <tr>
                  <td style="width:75%;border-right-width:0px;">
                    <img alt="." >
                        <xsl:if test= "(dsSchema/tblReport/BaseBackProblems = 'true')">
                          <xsl:attribute name="SRC">
                            <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                          </xsl:attribute>
                        </xsl:if>
                    </img>
                    &#160;<a style ="font-weight:bold">BACK PAIN</a>
                  </td>
                  <td style="width:15%;border-right-width:0;border-left-width:0;">
                    <xsl:value-of select="dsSchema/tblReport/BackResolvedDate"/>
                  </td>
                  <td style="border-left-width:0;">
                    <xsl:value-of select="dsSchema/tblReport/BackResolvedDate_Diff"/>
                  </td>
                </tr>
              </table>
            </td>
          </tr>

          <tr>
            <td colspan = "2">
              <table width="100%" border="1px" cellspacing="0">
                <tr>
                  <td style="width:75%;border-right-width:0px;">
                    <img alt="." >
                      
                        <xsl:if test= "(dsSchema/tblReport/BaseArthritisProblems = 'true')">
                          <xsl:attribute name="SRC">
                            <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                          </xsl:attribute>
                        </xsl:if>
                        
                    </img>
                    &#160;<a style ="font-weight:bold">ARTHRITIS</a>
                  </td>
                  <td style="width:15%;border-right-width:0;border-left-width:0;">
                    <xsl:value-of select="dsSchema/tblReport/ArthritisResolvedDate"/>
                  </td>
                  <td style="border-left-width:0;">
                    <xsl:value-of select="dsSchema/tblReport/ArthritisResolvedDate_Diff"/>
                  </td>
                </tr>
              </table>
          </td>
        </tr>

        <tr>
          <td colspan = "2">
              <table width="100%" border="1px" cellspacing="0">
                <tr>
                  <td style="width:75%;border-right-width:0px;">
                    <img alt="." >
                        <xsl:if test= "(dsSchema/tblReport/BaseCVDProblems = 'true')">
                          <xsl:attribute name="SRC">
                            <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                          </xsl:attribute>
                        </xsl:if>
                    </img>
                    &#160;<a style ="font-weight:bold">CARDIAC</a>
                  </td>
                  <td style="width:15%;border-right-width:0;border-left-width:0;">
                    <xsl:value-of select="dsSchema/tblReport/CVDLevelResolvedDate"/>
                  </td>
                  <td style="border-left-width:0;">
                    <xsl:value-of select="dsSchema/tblReport/CVDLevelResolvedDate_Diff"/>
                  </td>
                </tr>
              </table>
          </td>
        </tr>
        <tr>
          <td colspan="2">
            <br/>
          </td>
        </tr>
        <xsl:variable name="RootURL" select="dsSchema/tblReport/RootURL"/>
        <tr style="vertical-align:top">
          <td >
            <table border="1px" width="100%" cellspacing="0">
              <tr>
                <td colspan="3" style= "font-size:medium; font-weight:bold; Color:BROWN">Complications</td>
              </tr>
              <tr style="vertical-align:top;text-align:center">
                <td style="width:10%">Date</td>
                <td style="width:35%"/>
                <td style="width:5%">
                  Read<br/>Admitted
                </td>
              </tr>
              <xsl:for-each select="dsSchema/tblComplication">
                <tr>
                  <xsl:if test="position() mod 2 = 0">
                    <xsl:attribute name="style">Border:0;text-align:Left;background:AliceBlue;color:darkblue</xsl:attribute>
                  </xsl:if>

                  <xsl:if test="position() mod 2 != 0">
                    <xsl:attribute name="style">Border:0;text-align:Left;background:LightBlue;color:darkblue</xsl:attribute>
                  </xsl:if>
                  <td>
                    <xsl:value-of select="ComplicationDate"/></td>
                  <td>
                    <xsl:value-of select="Complication"/></td>
                  <td style="text-align:center">
                    <img >
                      <xsl:if test ="Readmitted='true'">
                        <xsl:attribute name="SRC">
                          <xsl:value-of select="$RootURL"/>img/tick.gif
                        </xsl:attribute>
                      </xsl:if>
                    </img>
                  </td>
                </tr>
              </xsl:for-each>
            </table>
          </td>
          <td>
            <table border="1px" width="100%" cellspacing="0">
              <tr>
                <td colspan="3" style= "font-size:medium; font-weight:bold; Color:BROWN">Events</td>
              </tr>
              <tr style="vertical-align:top;text-align:center">
                <td style="width:10%">Date</td>
                <td style="width:35%"/>
                <td style="width:5%">
                  Read<br/>Admitted
                </td>
              </tr>
              <xsl:for-each select="dsSchema/tblEvent">
                <tr>
                  <xsl:if test="position() mod 2 = 0">
                    <xsl:attribute name="style">Border:0;text-align:Left;background:AliceBlue;color:darkblue</xsl:attribute>
                  </xsl:if>

                  <xsl:if test="position() mod 2 != 0">
                    <xsl:attribute name="style">Border:0;text-align:Left;background:LightBlue;color:darkblue</xsl:attribute>
                  </xsl:if>
                  <td>
                    <xsl:value-of select="ComplicationDate"/>
                  </td>
                  <td>
                    <xsl:value-of select="Complication"/> 
                  </td>
                  <td style="text-align:center">
                    <img >
                      <xsl:if test ="Readmitted='true'">
                        <xsl:attribute name="SRC">
                          <xsl:value-of select="$RootURL"/>img/tick.gif
                        </xsl:attribute>
                      </xsl:if>
                    </img>
                  </td>
                </tr>
              </xsl:for-each>
            </table>
          </td>
        </tr>
      </table>

    </body>
    </html>
</xsl:template>

</xsl:stylesheet> 

