<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">

  <xsl:template match="/">
    <html>
    <body>
    <!--
        This is an XSLT template file. Fill in this area with the
        XSL elements which will transform your XML to XHTML.
    -->
      <table style="width:100%">
        <tr style = "font-size : large; font-weight:bold; Color:BLUE">
          <td colspan = "3" >
            Patient Name : <xsl:value-of select="dsSchema/tblPatientData/PatientName" />
          </td>
          <td style ="width:50%;text-align:right" >Operation Details</td>
        </tr>
        <tr>
          <td>Address : </td>
          <td colspan="2">
            <xsl:value-of select="dsSchema/tblPatientData/Address" />
          </td>
          <td />
        </tr>
        <tr>
          <td>Home Phone : </td>
          <td>
            <xsl:value-of select="dsSchema/tblPatientData/HomePhone" />
          </td>
          <td />
          <td />
        </tr>
        <tr>
          <td style="width:10%">Birth Date : </td>
          <td style="width:10%">
            <xsl:value-of select="dsSchema/tblPatientData/Birthdate" />
          </td>
          <td >
            Age : <xsl:value-of select = "dsSchema/tblPatientData/AGE"/>
          </td>
          <td />
        </tr>
        <tr>
          <td colspan="4">
            <hr style="border-top: 1px solid #ccc;" color="brown" size="3"/>
          </td>
        </tr>
        <tr>
          <td colspan = "4" style= "font-size:medium; font-weight:bold; Color:BROWN;font-Style:italic">
            <U>Initial Assessment</U>
          </td>
        </tr>
        <tr>
          <td colspan="4">
            <table style="width:100%" border="0">
              <tr>
                <td style="width:7%" >Height :</td>
                <td style="width:5%" >
                  <xsl:value-of select="round(dsSchema/tblPatientData/Height)"/>
                </td>
                <td style="width:10%" >
                  <xsl:value-of select="dsSchema/tblPatientData/HeightMeasurment"/>
                </td>
                <td style="width:17%" >Ideal Weight : </td>
                <td style="width:5%" >
                  <xsl:value-of select="round(dsSchema/tblPatientData/IdealWeight)"/>
                </td>
                <td style="width:10%" >
                  <xsl:value-of select="dsSchema/tblPatientData/WeightMeasurment"/>
                </td>
                <td rowspan = "3"/>
              </tr>

              <tr>
                <td>Weight : </td>
                <td>
                  <xsl:value-of select="round(dsSchema/tblPatientData/StartWeight)"/>
                </td>
                <td>
                  <xsl:value-of select="dsSchema/tblPatientData/WeightMeasurment"/>
                </td>
                <td>Target Weight : </td>
                <td>
                  <xsl:value-of select="round(dsSchema/tblPatientData/TargetWeight)"/>
                </td>
                <td>
                  <xsl:value-of select="dsSchema/tblPatientData/WeightMeasurment"/>
                </td>
              </tr>

              <tr>
                <td>BMI : </td>
                <td>
                  <xsl:value-of select="dsSchema/tblPatientData/StartBMI"/>
                </td>
                <td></td>
                <td>% above Ideal  Weight : </td>
                <td>
                  <xsl:value-of select="dsSchema/tblPatientData/Above_Ideal_Weight"/>
                </td>
                <td />
              </tr>
            </table>
          </td>
        </tr>
        <tr>
          <td colspan="5">
            <hr style="border-top: 1px solid #ccc;" color="brown" size="3"/>
          </td>
        </tr>
        <tr style= "font-size:large; font-weight:bold; font-Style:italic">
          <td colspan = "2" style="Color:BROWN;">
            <U>Operation Details</U>
          </td>
          <td>
            <xsl:value-of select = "dsSchema/tblPatientOperation/SugeryDesc" />
          </td>
        </tr>
        <tr>
          <td colspan="4">
            <br/>
          </td>
        </tr>
        <tr>
          <td colspan ="4">
            <table style="width:100%" border ="0">
              <tr>
                <td style ="width:7%">Date : </td>
                <td style ="width:23%">
                  <xsl:value-of select = "dsSchema/tblPatientOperation/strOperationDate" />
                </td>
                <td style="width:10%">Approach : </td>
                <td style="width:20%">
                  <xsl:value-of select = "dsSchema/tblPatientOperation/Approach" />
                </td>
                <td style="width:10%">Duration : </td>
                <td style="width:5%">
                  <xsl:value-of select = "dsSchema/tblPatientOperation/DaysInHospital" />
                </td>
                <td rowspan="2" style="width:25%"/>
              </tr>
              <tr>
                <td>Surgeon : </td>
                <td>
                  <xsl:value-of select = "dsSchema/tblPatientOperation/SurgeonName" />
                </td>
                <td>Category : </td>
                <td>
                  <xsl:value-of select = "dsSchema/tblPatientOperation/Category_Desc"/>
                </td>
                <td>BloodLoss : </td>
                <td>
                  <xsl:value-of select = "dsSchema/tblPatientOperation/BloodLoss"/>
                </td>
              </tr>
            </table>
          </td>
        </tr>
      </table>
      <br/>

      <xsl:choose>
        <!--OPLapBand Subreport-->
        <xsl:when test="number(dsSchema/tblPatientOperation/SurgeryType) = 1">
          <table style="width:100%" border ="0">
            <tr>
              <td style="width:17%">Lapband Serial Number :</td>
              <td style="width:15%">
                <xsl:value-of select = "dsSchema/tblPatientOperation/LapbandSerialNumber" />
              </td>
              <td style="width:15%">Reservoir Volume :</td>
              <td style="width:15%">
                <xsl:value-of select = "dsSchema/tblPatientOperation/BalloonVolumne" />
              </td>
              <td rowspan="2"/>
            </tr>
            <tr>
              <td>Resevior :</td>
              <td>
                <xsl:value-of select = "dsSchema/tblPatientOperation/ReservoirSite" />
              </td>
              <td>Posterior Fixarion :</td>
              <td>
                <input type = "checkbox" />
                <xsl:value-of select = "dsSchema/tblPatientOperation/PosteriorFixation" />
              </td>
            </tr>
            <tr>
              <td>Bandsize :</td>
              <td>
                <xsl:value-of select = "dsSchema/tblPatientOperation/BandSize" />
              </td>
            </tr>
          </table>
        </xsl:when>

        <!--OpRouxy Subreport-->
        <xsl:when test="number(dsSchema/tblPatientOperation/SurgeryType) = 2">
          <table style="width:100%" border ="0" cellspacing="0" cellpathing="0">
            <tr>
              <td style="width:15%">Limb Length :</td>
              <td style="width:15%">
                <xsl:value-of select = "dsSchema/tblPatientOperation/RouxLimbLength" />
              </td>
              <td style="width:15%">Retro / Antecolic</td>
              <td style="width:10%">
                <xsl:if test ="dsSchema/tblPatientOperation/RouxColic = 'R'">Retrocolic</xsl:if>
                <xsl:if test ="dsSchema/tblPatientOperation/RouxColic = 'A'">Antecolic</xsl:if>
              </td>
              <td rowspan="2"/>
            </tr>
            <tr>
              <td>Gastroenterostomy :</td>
              <td>
                <xsl:value-of select = "dsSchema/tblPatientOperation/Gastroenterostomy_Desc" />
              </td>
              <td>Retro / Antegastric :</td>
              <td>
                <xsl:if test ="dsSchema/tblPatientOperation/RouxGastric = 'R'">Retrogastric</xsl:if>
                <xsl:if test ="dsSchema/tblPatientOperation/RouxGastric = 'A'">Antegastric</xsl:if>
              </td>
            </tr>
          </table>
          <xsl:call-template name="SubOperationDetailReport"/>
        </xsl:when>

        <!--OpVGB Subreport-->
        <xsl:when test="number(dsSchema/tblPatientOperation/SurgeryType) = 5">
          <table style="width:100%;height:30px" border ="0" cellspacing ="0" cellpadding="0">
            <tr>
              <td style="width:10%">Stomal Wrap :</td>
              <td style="width:12%">
                <xsl:value-of select = "dsSchema/tblPatientOperation/VBGStomaWrap_Desc" />
              </td>
              <td style="width:10%">Stomal Size :</td>
              <td style="width:10%">
                <xsl:value-of select = "dsSchema/tblPatientOperation/VBGStomaSize" />
              </td>
              <td/>
            </tr>
          </table>
          <xsl:call-template name="SubOperationDetailReport"/>
        </xsl:when>

        <!--OpBD Subreport-->
        <xsl:when test="number(dsSchema/tblPatientOperation/SurgeryType) = 6">
          <table style="width:100%" border="1" cellpadding="0" cellspacing="0">
            <tr >
              <td style="width:12%">Illegal Length :</td>
              <td style="width:10%">
                <xsl:value-of select = "dsSchema/tblPatientOperation/BPDIlealLength" />
              </td>
              <td style="width:12%">Channel Length :</td>
              <td style="width:10%">
                <xsl:value-of select = "dsSchema/tblPatientOperation/BPDChannelLength" />
              </td>
              <td style="width:12%">Duodenal Switch</td>
              <td style="width:5%">
                <xsl:value-of select = "dsSchema/tblPatientOperation/BPDDuodenalSwitch" />
              </td>
              <td/>
            </tr>
          </table>
          <xsl:call-template name="SubOperationDetailReport"/>

        </xsl:when>
        
        <!--OpLoop Subreport-->
        <xsl:when test="number(dsSchema/tblPatientOperation/SurgeryType) = 61">
          <table style="width:100%">
            <tr>
              <td style="width:15%">Ileal Segment :</td>
              <td style="width:10%">
                <xsl:value-of select = "dsSchema/tblPatientOperation/LGBilealSegment" />&#160;&#160;cms
              </td>
              <td style="width:15%">Omental Patch :</td>
              <td style="width:5%">
                <xsl:value-of select="dsSchema/tblPatientOperation/LGBomentalpatch"/>
              </td>
              <td style="width:15%">Reinforced Sutures :</td>
              <td style="width:5%">
                <xsl:value-of select="dsSchema/tblPatientOperation/LGBreinforcedSutures"/>
              </td>
              <td/>
            </tr>
          </table>
        </xsl:when>
      </xsl:choose>
      <br/>
      
      <hr style="border-top: 1px solid #ccc;" color="brown" size="3"/>

      <p style= "font-size:medium; font-weight:bold; Color:BROWN;font-Style:italic">
        <U>Complications / Events</U>
      </p>
      <table style="width:75%" border ="0" cellspacing ="0" cellpadding="0">
        <tr style="background-color:#999;height:30px">
          <td style="width:20%">Date</td>
          <td style="width:20%">Wks PO</td>
          <td >Complication</td>
        </tr>
        <xsl:for-each select="dsSchema/tblPatientComplication">
          <tr style ="height:25px">
            <td>
              <xsl:value-of select = "strComplicationDate" />
            </td>
            <td>
              <xsl:value-of select = "Weeks" />
            </td>
            <td>
              <xsl:value-of select = "Complication" />
            </td>
          </tr>
        </xsl:for-each>
      </table>
    </body>
  </html>
</xsl:template>

<xsl:template name = "SubOperationDetailReport">
  <xsl:if test="dsSchema/tblPatientOperation/PreviousSurgery != 'false'">
    <br/>
    <table style="width:100%" border="0" cellpadding ="0" cellspacing="0">
      <tr>
        <td colspan = "4">Past Adbominal</td>
      </tr>
      <tr>
        <td style="width:5%"></td>
        <td style="width:30%">
          1:&#160;<xsl:value-of select = "dsSchema/tblPatientOperation/PrevAbdoSurgery1_Desc" />
        </td>
        <td style="width:30%">
          2:&#160;<xsl:value-of select = "dsSchema/tblPatientOperation/PrevAbdoSurgery2_Desc" />
        </td>
        <td style="width:30%">
          3:&#160;<xsl:value-of select = "dsSchema/tblPatientOperation/PrevAbdoSurgery3_Desc" />
        </td>
      </tr>
      <tr>
        <td >Notes</td>
        <td colspan = "3">
          <xsl:value-of select = "dsSchema/tblPatientOperation/PrevAbdoSurgeryNotes" />
        </td>
      </tr>

    </table>
  </xsl:if>
  <xsl:if test="dsSchema/tblPatientOperation/PrevPelvicSurgery != 'false'">
  <br/>
  <table style="width:100%" border="0" cellpadding ="0" cellspacing="0">
    <tr>
      <td colspan = "4">Past Pelvic</td>
    </tr>
    <tr>
      <td style="width:5%"></td>
      <td style="width:30%">
        1:&#160;<xsl:value-of select = "dsSchema/tblPatientOperation/PrevPelvicSurgery1_Desc" />
      </td>
      <td style="width:30%">
        2:&#160;<xsl:value-of select = "dsSchema/tblPatientOperation/PrevPelvicSurgery2_Desc" />
      </td>
      <td style="width:30%">
        3:&#160;<xsl:value-of select = "dsSchema/tblPatientOperation/PrevPelvicSurgery3_Desc" />
      </td>
    </tr>
    <tr>
      <td>Notes</td>
      <td colspan = "4">
        <xsl:value-of select = "dsSchema/tblPatientOperation/PrevPelvicSurgeryNotes" />
      </td>
    </tr>
  </table>
  </xsl:if>
  <xsl:if test="dsSchema/tblPatientOperation/ComcomitantSurgery != 'false'">
  <br/>
  <table  style="width:100%" border="0" cellpadding ="0" cellspacing="0">
    <tr>
      <td colspan = "4">Comcomitant Surgery</td>
    </tr>
    <tr>
      <td style="width:5%"></td>
      <td style="width:30%">
        1:&#160;<xsl:value-of select = "dsSchema/tblPatientOperation/ComcomitantSurgery1_Desc" />
      </td>
      <td style="width:30%">
        2:&#160;<xsl:value-of select = "dsSchema/tblPatientOperation/ComcomitantSurgery2_Desc" />
      </td>
      <td style="width:30%">
        3:&#160;<xsl:value-of select = "dsSchema/tblPatientOperation/ComcomitantSurgery3_Desc" />
      </td>
    </tr>
    <tr>
      <td>Notes</td>
      <td colspan = "4">
        <xsl:value-of select = "dsSchema/tblPatientOperation/ComcomitantSurgeryNotes" />
      </td>
    </tr>
  </table>
  </xsl:if>
  </xsl:template>

</xsl:stylesheet> 

