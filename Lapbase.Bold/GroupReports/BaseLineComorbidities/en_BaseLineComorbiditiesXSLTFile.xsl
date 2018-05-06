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
        <tr >
          <td style= "font-size:medium; font-weight:bold;">
            <xsl:value-of select="dsSchema/tblReport/Address"/></td>
          <td />
        </tr>
        <tr>
          <td>
            <a style= "font-size:medium; font-weight:bold;">BOD : </a><xsl:value-of select="dsSchema/tblReport/Birthdate"/>
            <a style= "font-size:medium; font-weight:bold;">AGE : </a><xsl:value-of select="dsSchema/tblReport/AGE"/>
          </td>
          <td/>
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
                <td  style ="border-bottom-width:0;border-right-width:0;">
                  <a>Height : </a><xsl:value-of select="dsSchema/tblReport/Height"/>&#160;&#160;
                  <xsl:value-of select="dsSchema/tblReport/HeightMeasurment"/>
                </td>
                <td style ="border-bottom-width:0;border-left-width:0;border-right-width:0;">
                  <a>BMI : </a>
                  <xsl:value-of select="dsSchema/tblReport/InitBMI"/>
                </td>
                <td  style ="border-bottom-width:0;border-left-width:0;">
                  <a>Target Weight : </a><xsl:value-of select="dsSchema/tblReport/TargetWeight"/>&#160;&#160;<xsl:value-of select="dsSchema/tblReport/WeightMeasurment"/>
                </td>
              </tr>

              <tr  style="Height:30px">
                <td  style ="border-top-width:0;border-right-width:0;">
                  <a>Weight : </a><xsl:value-of select="dsSchema/tblReport/StartWeight"/>&#160;&#160;<xsl:value-of select="dsSchema/tblReport/WeightMeasurment"/>
                </td>
                <td style ="border-top-width:0;border-left-width:0;border-right-width:0;">
                  <a>Ideal Weight: </a><xsl:value-of select="dsSchema/tblReport/IdealWeight"/>&#160;&#160;<xsl:value-of select="dsSchema/tblReport/WeightMeasurment"/>
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
          <td colspan = "2">
            <table width = "100%" >
              <tr style="height:40px">
                <td colspan = "7">
                  <a style= "font-size:large; font-weight:bold;color:RED">Comorbidties</a>&#160;&#160;&#160;&#160;
                  <a style= "font-weight:bold">comorbidites present indicated by </a>&#160;&#160;
                  <img alt="." ></img>
                </td>
              </tr>
              <tr >
                <td style ="width:20px" rowspan = "6"/>
                <td colspan="6">
                  <table width="100%" border="1px" cellspacing="0">
                    <tr style="height:40px">
                      <td style ="width:20px;border-right-width:0;border-bottom-width:0;">
                        <img alt="." >
                          <xsl:if test= "dsSchema/tblReport/BaseSystolicBP != 0 or dsSchema/tblReport/BaseDiastolicBP != 0 ">
                            <xsl:attribute name="SRC">
                              <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif</xsl:attribute>
                          </xsl:if>
                        </img>
                      </td>
                      <td style ="width:30%;border-right-width:0;border-bottom-width:0;border-left-width:0;" >
                        <a style= "font-size:medium; font-weight:bold; text-decoration:underline;">BLOOD PRESSURE</a>
                      </td>
                      <td style ="border-bottom-width:0;border-left-width:0;">
                        <xsl:value-of select="dsSchema/tblReport/BaseSystolicBP"/> / <xsl:value-of select="dsSchema/tblReport/BaseDiastolicBP"/>
                      </td>
                    </tr>
                    <tr>
                      <td style ="border-right-width:0;border-top-width:0;"/>
                      <td colspan="2" style ="border-left-width:0;border-top-width:0;" >
                        <xsl:value-of select="dsSchema/tblReport/BaseBloodPressureRxDetails"/>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
              
              <tr >
                <td colspan="6">
                  <table width="100%" border ="1px" cellspacing="0">
                    <tr style="height:40px">
                      <td rowspan = "2" style ="width:20px;border-right-width:0;" >
                        <img alt="." >
                          <xsl:if test= "(dsSchema/tblReport/BaseTotalCholesterol != 0) or (dsSchema/tblReport/BaseHDLCholesterol != 0) or (dsSchema/tblReport/BaseTriglycerides != 0)">
                            <xsl:attribute name="SRC">
                              <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                            </xsl:attribute>
                          </xsl:if>
                        </img>
                      </td>
                      <td style="width:30%;border-bottom-width:0;border-right-width:0;border-left-width:0;">
                        <a style= "font-size:medium; font-weight:bold; text-decoration:underline;">LIPIDS</a>
                      </td>
                      <td style="border-bottom-width:0;border-right-width:0;border-left-width:0;">
                        <a style ="font-weight:bold;">Chol. </a>
                        <xsl:value-of select="dsSchema/tblReport/BaseTotalCholesterol"/>
                      </td>
                      <td style="border-bottom-width:0;border-right-width:0;border-left-width:0;">
                        <a style ="font-weight:bold;">HDL. </a>
                        <xsl:value-of select="dsSchema/tblReport/BaseHDLCholesterol"/>
                      </td>
                      <td style="border-bottom-width:0;border-right-width:0;border-left-width:0;">
                        <a style ="font-weight:bold;">LDL. </a>
                        <xsl:value-of select="dsSchema/tblReport/BaseLDLCholesterol"/>
                      </td>
                      <td style="border-bottom-width:0;border-left-width:0;">
                        <a style ="font-weight:bold;">Trig. </a>
                        <xsl:value-of select="dsSchema/tblReport/BaseTriglycerides"/>
                      </td>
                    </tr>
                    <tr>
                      <td colspan = "5" style="border-top-width:0;border-left-width:0;">
                        <xsl:value-of select="dsSchema/tblReport/BaseLipidRxDetails"/>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>

              <tr>
                <td colspan="6">
                  <table width="100%" border="1px" cellspacing="0">
                    <tr style="height:40px">
                      <td rowspan = "2" style ="width:20px;border-right-width:0;" >
                        <img alt="." >
                          <xsl:if test= "(dsSchema/tblReport/BaseFBloodGlucose != 0)">
                            <xsl:attribute name="SRC">
                              <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                            </xsl:attribute>
                          </xsl:if>
                        </img>
                      </td>
                      <td style="width:30%;border-bottom-width:0;border-right-width:0;border-left-width:0;">
                        <a style= "font-size:medium; font-weight:bold; text-decoration:underline;">BLOOD SUGAR</a>
                      </td>
                      <td style="border-bottom-width:0;border-left-width:0;">
                        <xsl:value-of select="dsSchema/tblReport/BaseFBloodGlucose"/>
                      </td>
                    </tr>
                    <tr>
                      <td colspan = "5" style="border-top-width:0;border-left-width:0;">
                        <xsl:value-of select="dsSchema/tblReport/BaseDiabetesRxDetails"/>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td colspan="6">
                  <table width="100%" border="1px" cellspacing="0">
                    <tr style="height:40px">
                      <td rowspan = "2" style ="width:20px;border-right-width:0;" >
                        <img alt="." >
                          <xsl:if test= "(dsSchema/tblReport/BaseAsthmaLevel != 0)">
                            <xsl:attribute name="SRC">
                              <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                            </xsl:attribute>
                          </xsl:if>
                        </img>
                      </td>
                      <td style="width:30%;border-bottom-width:0;border-right-width:0;border-left-width:0;">
                        <a style= "font-size:medium; font-weight:bold; text-decoration:underline;">ASTHMA</a>
                      </td>
                      <td style="border-bottom-width:0;border-left-width:0;">
                        <xsl:value-of select="dsSchema/tblReport/strBaseAsthmaLevel"/>
                      </td>
                    </tr>
                    <tr>
                      <td colspan = "5" style="border-top-width:0;border-left-width:0;">
                        <xsl:value-of select="dsSchema/tblReport/BaseAsthmaDetails"/>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>

              <tr>
                <td colspan="6">
                  <table width="100%" border="1px" cellspacing="0">
                    <tr style="height:40px">
                      <td rowspan = "2" style ="width:20px;border-right-width:0;" >
                        <img alt="." >
                          <xsl:if test= "(dsSchema/tblReport/BaseRefluxLevel != 0)">
                            <xsl:attribute name="SRC">
                              <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                            </xsl:attribute>
                          </xsl:if>
                        </img>
                      </td>
                      <td style="width:30%;border-bottom-width:0;border-right-width:0;border-left-width:0;">
                        <a style= "font-size:medium; font-weight:bold; text-decoration:underline;">REFLUX</a>
                      </td>
                      <td style="border-bottom-width:0;border-left-width:0;">
                        <xsl:value-of select="dsSchema/tblReport/strBaseRefluxLevel"/>
                      </td>
                    </tr>
                    <tr>
                      <td colspan = "5" style="border-top-width:0;border-left-width:0;">
                        <xsl:value-of select="dsSchema/tblReport/BaseRefluxDetails"/>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>

              <tr>
                <td colspan="6">
                  <table width="100%" border="1px" cellspacing="0">
                    <tr style="height:40px">
                      <td rowspan = "2" style ="width:20px;border-right-width:0;" >
                        <img alt="." >
                          <xsl:if test= "(dsSchema/tblReport/BaseSleepLevel != 0)">
                            <xsl:attribute name="SRC">
                              <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                            </xsl:attribute>
                          </xsl:if>
                        </img>
                      </td>
                      <td style="width:30%;border-bottom-width:0;border-right-width:0;border-left-width:0;">
                        <a style= "font-size:medium; font-weight:bold; text-decoration:underline;">SLEEP</a>
                      </td>
                      <td style="border-bottom-width:0;border-left-width:0;">
                        <xsl:value-of select="dsSchema/tblReport/strBaseSleepLevel"/>
                      </td>
                    </tr>
                    <tr>
                      <td colspan = "5" style="border-top-width:0;border-left-width:0;">
                        <xsl:value-of select="dsSchema/tblReport/BaseSleepDetails"/>
                      </td>
                    </tr>
                  </table>
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
          <td style ="border-width:1px">
            <table width="100%" border="1px" cellspacing="0">
              <tr>
                <td>
                  <img alt="." >
                    <xsl:if test= "(dsSchema/tblReport/BaseFertilityProblems = 'true')">
                      <xsl:attribute name="SRC">
                        <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                      </xsl:attribute>
                    </xsl:if>
                  </img>
                  &#160;<a style ="font-weight:bold">FERTILITY</a><br /><br />
                  <xsl:value-of select="dsSchema/tblReport/BaseFertilityDetails"/>
                </td>
              </tr>
            </table>
          </td>
          <td style ="border-width:1px">
            <table width="100%" border="1px" cellspacing="0">
            <tr>
              <td>
                <img alt="." >
                  <xsl:if test= "(dsSchema/tblReport/BaseArthritisProblems = 'true')">
                    <xsl:attribute name="SRC">
                      <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                    </xsl:attribute>
                  </xsl:if>
                </img>
                &#160;<a style ="font-weight:bold">ARTHRITIS</a><br /><br />
                  <xsl:value-of select="dsSchema/tblReport/BaseArthritisDetails"/>
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
          <td style ="border-width:1px">
            <table width="100%" border="1px" cellspacing="0">
              <tr>
                <td>
                  <img alt="." >
                    <xsl:if test= "(dsSchema/tblReport/BaseIncontinenceProblems = 'true')">
                      <xsl:attribute name="SRC">
                        <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                      </xsl:attribute>
                    </xsl:if>
                  </img>
                  &#160;<a style ="font-weight:bold">INCONTINENCE</a><br /><br />
                  <xsl:value-of select="dsSchema/tblReport/BaseIncontinenceDetails"/>
                </td>
              </tr>
            </table>
          </td>
          <td style ="border-width:1px">
            <table width="100%" border="1px"  cellspacing="0">
            <tr>
              <td>
                <img alt="." >
                  <xsl:if test= "(dsSchema/tblReport/BaseCVDProblems = 'true')">
                    <xsl:attribute name="SRC">
                      <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                    </xsl:attribute>
                  </xsl:if>
                </img>
                &#160;<a style ="font-weight:bold">CARDIAC</a><br /><br />
                  <xsl:value-of select="dsSchema/tblReport/BaseCVDDetails"/>
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
          <td style ="border-width:1px">
            <table width="100%" border="1px" cellspacing="0">
              <tr>
                <td>
                  <img alt="." >
                    <xsl:if test= "(dsSchema/tblReport/BaseBackProblems = 'true')">
                      <xsl:attribute name="SRC">
                        <xsl:value-of select="dsSchema/tblReport/RootURL"/>img/tick.gif
                      </xsl:attribute>
                    </xsl:if>
                  </img>
                  &#160;<a style ="font-weight:bold">BACK PAIN</a><br /><br />
                  <xsl:value-of select="dsSchema/tblReport/BaseBackDetails"/>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
    </body>
  </html>
</xsl:template>

</xsl:stylesheet> 

