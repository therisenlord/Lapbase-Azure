<?xml version="1.0" encoding="utf-8"?>
<!--
<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">
-->
<xsl:stylesheet 
		xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
		xmlns:xs="http://www.w3.org/2001/XMLSchema"
		xmlns:fn="http://www.w3.org/2005/02/xpath-functions"
		version="2.0"
		xmlns:msxsl="urn:schemas-microsoft-com:xslt"  >
		
	<xsl:template match="/">
  <html>
    <head>
	  <title>LapBase - A Data Manager for Bariatric Surgery</title>
      <META http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
      
      <style type="text/css" media="screen">
        hr {
        border: 0px;
        border-top: 4px solid #d51c1c;
        height: 1px;
        background-color: #fff;
        color: #fff;
        margin: 10px 0;
        }
        table {
        padding: 0;
        }
        table tr {
        padding: 0;
        }
        table td {
        padding: 0;
        }
        .noPrint {
        margin: 0 0 20px 0;
        }
        .printPage {
        border: 1px solid #ccc;
        width: 100%
        }
        .row01 {
        padding: 5px 0;
        background-color: #fdf1c1;
        }
        .row02 {
        padding: 5px 0;
        background-color: #f7eda8;
        border-bottom: 1px solid #e0d385;
        }
        .rowComment {
        padding: 5px 0;
        background-color: #ffc;
        border-bottom: 1px solid #ffc;
        }
      </style>
    <style type="text/css" media="print">
      hr {
      border: 0px;
      border-top: 4px solid #d51c1c;
      height: 1px;
      background-color: #fff;
      color: #fff;
      margin: 10px 0;
      }
      .noPrint {
      display: none;
      }
      .printPage {
      margin: 0 auto;
      border: 1px solid #666;
      width: 98%
      }
      .row01 {
      padding: 5px 0;
      border-bottom: 1px solid #ccc;
      }
      .row02 {
      padding: 5px 0;
      border-bottom: 1px solid #ccc;
      }
      .rowComment {
      padding: 5px 0;
      border-bottom: 1px solid #ccc;
      }
    </style>
  </head>
  <body>
    <table width ="100%" class = "printPage">
      <tr>
        <td colspan="3">
          <xsl:if test ="dsSchema/tblPatientData/LogoPath != ''">
            <img>
              <xsl:attribute name="src">
                <xsl:value-of select="dsSchema/tblPatientData/LogoPath"/>
              </xsl:attribute>
            </img>
          </xsl:if>
        </td>
      </tr>
      <tr height="40">
        <td colspan="3">
          <xsl:attribute name="style">
            font-size : large; font-weight:bold; Color:BLUE; 
            padding:<xsl:value-of select="dsSchema/tblPatientData/LetterheadMargin"/>cm 0 0 0
          </xsl:attribute>
			<label id ="lblFollowUpDetailSheet">
				<xsl:for-each select="/dsSchema/tblCaptions">
					<xsl:if test ="Field_ID = 'lblFollowUpDetailSheet'">
						<xsl:value-of select="FIELD_CAPTION"/>
					</xsl:if>
				</xsl:for-each></label>
		</td>
      </tr>
      <tr height="30">
        <td style= "font-size:medium; font-weight:bold; Color:BROWN">
            <xsl:value-of select='translate(dsSchema/tblPatientData/PatientName, "`", "&apos;")'/>
		</td>
        <td >
			<label id ="lblPatientID">
				<xsl:for-each select="/dsSchema/tblCaptions">
					<xsl:if test ="Field_ID = 'lblPatientID'">
						<xsl:value-of select="FIELD_CAPTION"/>
					</xsl:if>
				</xsl:for-each>
			</label><xsl:value-of select="dsSchema/tblPatientData/Patient_CustomID"/></td>
        <td>
			<label id ="lblAge">
				<xsl:for-each select="/dsSchema/tblCaptions">
					<xsl:if test ="Field_ID = 'lblAge'">
						<xsl:value-of select="FIELD_CAPTION"/>
					</xsl:if>
				</xsl:for-each>
			</label><xsl:value-of select="dsSchema/tblPatientData/AGE"/></td>
      </tr>
      <tr>
        <td colspan = "2"  height="25">
          <xsl:value-of select="dsSchema/tblPatientData/Address"/></td>
        <td>
			<label id ="lblDateOfBirth">
				<xsl:for-each select="/dsSchema/tblCaptions">
					<xsl:if test ="Field_ID = 'lblDateOfBirth'">
						<xsl:value-of select="FIELD_CAPTION"/>
					</xsl:if>
				</xsl:for-each>
			</label><xsl:value-of select="dsSchema/tblPatientData/Birthdate"/></td>
      </tr>
      <tr height="25">
        <td style = "width:35%">
			<label id ="lblHomePhone">
				<xsl:for-each select="/dsSchema/tblCaptions">
					<xsl:if test ="Field_ID = 'lblHomePhone'">
						<xsl:value-of select="FIELD_CAPTION"/>
					</xsl:if>
				</xsl:for-each>
			</label><xsl:value-of select="dsSchema/tblPatientData/HomePhone"/>
        </td>
        <td style = "width:35%">
			<label id ="lblWorkPhone">
				<xsl:for-each select="/dsSchema/tblCaptions">
					<xsl:if test ="Field_ID = 'lblWorkPhone'">
						<xsl:value-of select="FIELD_CAPTION"/>
					</xsl:if>
				</xsl:for-each>
			</label><xsl:value-of select="dsSchema/tblPatientData/WorkPhone"/>
        </td>
        <td style = "width:30%">
			<label id="lblMobile">
				<xsl:for-each select="/dsSchema/tblCaptions">
					<xsl:if test ="Field_ID = 'lblMobile'">
						<xsl:value-of select="FIELD_CAPTION"/>
					</xsl:if>
				</xsl:for-each>
			</label><xsl:value-of select="dsSchema/tblPatientData/MobilePhone"/>
        </td>
      </tr>
      <tr height="25">
        <td colspan = "3">
			<label id="lblEmail">
				<xsl:for-each select="/dsSchema/tblCaptions">
					<xsl:if test ="Field_ID = 'lblEmail'">
						<xsl:value-of select="FIELD_CAPTION"/>
					</xsl:if>
				</xsl:for-each>
			</label><xsl:value-of select="dsSchema/tblPatientData/EmailAddress"/></td>
      </tr>
      <tr>
        <td>
			<label id="lblReferredby">
				<xsl:for-each select="/dsSchema/tblCaptions">
					<xsl:if test ="Field_ID = 'lblReferredby'">
						<xsl:value-of select="FIELD_CAPTION"/>
					</xsl:if>
				</xsl:for-each>
			</label>   1) <xsl:value-of select="dsSchema/tblPatientData/RefDr1"/></td>
        <td>2) <xsl:value-of select="dsSchema/tblPatientData/RefDr2"/></td>
        <td>3) <xsl:value-of select="dsSchema/tblPatientData/RefDr3"/></td>
      </tr>

      <tr>
        <td colspan="3">
          <hr/>
        </td>
      </tr>

      <tr>
        <td colspan="3">
          <table style="width:100%" border="0" bordercolor="brown" cellpadding="0" cellspacing="0">
            <tr>
              <td>
                <table style="width:100%" border="0">
                  <tr >
                    <td style="width:33%">
                      <xsl:value-of select="dsSchema/tblPatientData/SurgeryType_Desc"/>
                    </td>
                    <td style="width:33%">
                      <xsl:value-of select="dsSchema/tblPatientData/Approach"/>
                    </td>
                    <td style="width:33%">
                      <xsl:value-of select="dsSchema/tblPatientData/Category_Desc"/>
                    </td>
                  </tr>
                  <tr >
                    <td>
						<label id="lblOperationDate">
							<xsl:for-each select="/dsSchema/tblCaptions">
								<xsl:if test ="Field_ID = 'lblOperationDate'">
									<xsl:value-of select="FIELD_CAPTION"/>
								</xsl:if>
							</xsl:for-each>
						</label><xsl:value-of select="dsSchema/tblPatientData/LapBandDate"/>
                    </td>
                    <td colspan = "2">
                      <xsl:value-of select="dsSchema/tblPatientData/MainDr"/>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>

      <tr>
        <td colspan="3">
          <hr/>
        </td>
      </tr>
      <tr height="25">
        <td colspan = "2" ><h3 id="lblInitialAssessment">
			<xsl:for-each select="/dsSchema/tblCaptions">
				<xsl:if test ="Field_ID = 'lblInitialAssessment'">
					<xsl:value-of select="FIELD_CAPTION"/>
				</xsl:if>
			</xsl:for-each>
		</h3></td>
        <td rowspan = "5">
          <xsl:if test="IncludePhoto = '1'">
            <div style="Height:100; width : 100;overflow:hidden">
              <img style="Height:100; width : 100">
                <xsl:attribute name="src">
                  <xsl:value-of select="dsSchema/tblPatientData/LastImageLocation"/>
                </xsl:attribute>
              </img>
            </div>
          </xsl:if>
        </td>
      </tr>
      <tr>
        <td colspan="3">
          <table style="width:50%" border="0" cellpadding="0" cellspacing="0">
            <tr style="height:20px">
              <td style="width:12%" id="lblHeight">
				  <xsl:for-each select="/dsSchema/tblCaptions">
					  <xsl:if test ="Field_ID = 'lblHeight'">
						  <xsl:value-of select="FIELD_CAPTION"/>
					  </xsl:if>
				  </xsl:for-each>
			  </td>
              <td style="width:8%" >
                <xsl:value-of select="round(dsSchema/tblPatientData/Height)"/>
              </td>
              <td style="width:7%" >
                <xsl:value-of select="dsSchema/tblPatientData/HeightMeasurment"/>
              </td>
              <td style="width:20%" id="lblIdealWeight">
				  <xsl:for-each select="/dsSchema/tblCaptions">
					  <xsl:if test ="Field_ID = 'lblIdealWeight'">
						  <xsl:value-of select="FIELD_CAPTION"/>
					  </xsl:if>
				  </xsl:for-each>
			  </td>
              <td style="width:8%" >
                <!--<xsl:value-of select="round(dsSchema/tblPatientData/IdealWeight)"/>-->
				  <xsl:call-template name = "fnLoadNumber">
					  <xsl:with-param name ="numValue" select="dsSchema/tblPatientData/IdealWeight"/>
				  </xsl:call-template>
              </td>
              <td style="width:7%" >
                <xsl:value-of select="dsSchema/tblPatientData/WeightMeasurment"/>
              </td>
            </tr>
            
            <tr style="height:20px">
              <td id ="lblWeight">
				  <xsl:for-each select="/dsSchema/tblCaptions">
					  <xsl:if test ="Field_ID = 'lblWeight'">
						  <xsl:value-of select="FIELD_CAPTION"/>
					  </xsl:if>
				  </xsl:for-each>
			  </td>
              <td>
                <!--<xsl:value-of select="round(dsSchema/tblPatientData/StartWeight)"/>-->
				  <xsl:call-template name = "fnLoadNumber">
					  <xsl:with-param name ="numValue" select="dsSchema/tblPatientData/StartWeight"/>
				  </xsl:call-template>
              </td>
              <td>
                <xsl:value-of select="dsSchema/tblPatientData/WeightMeasurment"/>
              </td>
              <td id ="lblTargetWeight">
				  <xsl:for-each select="/dsSchema/tblCaptions">
					  <xsl:if test ="Field_ID = 'lblTargetWeight'">
						  <xsl:value-of select="FIELD_CAPTION"/>
					  </xsl:if>
				  </xsl:for-each>
			  </td>
              <td>
                <!--<xsl:value-of select="round(dsSchema/tblPatientData/TargetWeight)"/>-->
				  <xsl:call-template name = "fnLoadNumber">
					  <xsl:with-param name ="numValue" select="dsSchema/tblPatientData/TargetWeight"/>
				  </xsl:call-template>
              </td>
              <td>
                <xsl:value-of select="dsSchema/tblPatientData/WeightMeasurment"/>
				  
              </td>
            </tr>

            <tr style="height:20px">
              <td id="lblBMI">
				  <xsl:for-each select="/dsSchema/tblCaptions">
					  <xsl:if test ="Field_ID = 'lblBMI'">
						  <xsl:value-of select="FIELD_CAPTION"/>
					  </xsl:if>
				  </xsl:for-each>
			  </td>
              <td>
                <!--<xsl:value-of select="dsSchema/tblPatientData/StartBMI"/>-->
				  <xsl:call-template name = "fnLoadNumber">
					  <xsl:with-param name ="numValue" select="dsSchema/tblPatientData/StartBMI"/>
				  </xsl:call-template>
              </td>
              <td></td>
              <td id="lblaboveIdealWeight">
				  <xsl:for-each select="/dsSchema/tblCaptions">
					  <xsl:if test ="Field_ID = 'lblaboveIdealWeight'">
						  <xsl:value-of select="FIELD_CAPTION"/>
					  </xsl:if>
				  </xsl:for-each>
			  </td>
              <td>
                <xsl:value-of select="dsSchema/tblPatientData/Above_Ideal_Weight"/>
              </td>
              <td />
            </tr>
          </table>
        </td>
      </tr>
      
      <tr>
        <td colspan = "3" >
          <hr/>
        </td>
      </tr>
      
      <xsl:if test ="dsSchema/tblPatientData/IncludePatientNote = '1'">
        <tr height="25">
          <td ><h3 id ="lblPatientNotes">
			  <xsl:for-each select="/dsSchema/tblCaptions">
				  <xsl:if test ="Field_ID = 'lblPatientNotes'">
					  <xsl:value-of select="FIELD_CAPTION"/>
				  </xsl:if>
			  </xsl:for-each>
		  </h3></td>
        </tr>
        <tr>
          <td colspan = "3">
			  <!--<xsl:value-of select="dsSchema/tblPatientData/PatientNotes"/>-->
			  <xsl:call-template name = "break">
				  <xsl:with-param name ="text" select="dsSchema/tblPatientData/PatientNotes"/>
			  </xsl:call-template>

		  </td>
        </tr>
      </xsl:if>
      <tr height="5x" style="height:5px">
        <td colspan = "3">
          <hr style="border-top: 1px solid #ccc;" color="brown" size="3"/>
        </td>
      </tr>
      <tr>
        <td colspan = "3">
          <table style="width:100%" cellpadding="0" cellspacing = "0" border="0">
            <tr style = "text-align:center;height:30px;font-family:Arial">
              <td colspan = "2" />
              <td colspan = "3" id ="lblWeight_TC">
				  <xsl:for-each select="/dsSchema/tblCaptions">
					  <xsl:if test ="Field_ID = 'lblWeight_TC'">
						  <xsl:value-of select="FIELD_CAPTION"/>
					  </xsl:if>
				  </xsl:for-each>
			  </td>
              <td colspan = "6" />
            </tr>
            <tr style = "text-align:Left; height:25px; font-size:small; font-family:Arial">
              <td style = "width:15%" />
              <td style = "width:15%" id="lblDate_TC">
				  <xsl:for-each select="/dsSchema/tblCaptions">
					  <xsl:if test ="Field_ID = 'lblDate_TC'">
						  <xsl:value-of select="FIELD_CAPTION"/>
					  </xsl:if>
				  </xsl:for-each>
			  </td>
              <td style = "width:10%" id="lblWeeks_TC">
				  <xsl:for-each select="/dsSchema/tblCaptions">
					  <xsl:if test ="Field_ID = 'lblWeeks_TC'">
						  <xsl:value-of select="FIELD_CAPTION"/>
					  </xsl:if>
				  </xsl:for-each>
			  </td>
              <td style = "width:10%" id="lblCurrent_TC">
				  <xsl:for-each select="/dsSchema/tblCaptions">
					  <xsl:if test ="Field_ID = 'lblCurrent_TC'">
						  <xsl:value-of select="FIELD_CAPTION"/>
					  </xsl:if>
				  </xsl:for-each>
			  </td>
              <td style = "width:10%" id="lblLoss_TC">
                <xsl:choose>
                  <xsl:when test ="dsSchema/tblPatientVisit/VisitWeeksFlag = '0'">
                    <xsl:for-each select="/dsSchema/tblCaptions">
                      <xsl:if test ="Field_ID = 'lblLoss_TC'">
                        <xsl:value-of select="FIELD_CAPTION"/>
                      </xsl:if>
                    </xsl:for-each>
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:for-each select="/dsSchema/tblCaptions">
                      <xsl:if test ="Field_ID = 'lblTotalLoss_TC'">
                        <xsl:value-of select="FIELD_CAPTION"/>
                      </xsl:if>
                    </xsl:for-each>
                  </xsl:otherwise>
                </xsl:choose>
			  </td>
              <td style = "width:10%" id="lblBMI_TC">
				  <xsl:for-each select="/dsSchema/tblCaptions">
					  <xsl:if test ="Field_ID = 'lblBMI_TC'">
						  <xsl:value-of select="FIELD_CAPTION"/>
					  </xsl:if>
				  </xsl:for-each>
			  </td>
              <td style = "width:10%" id="lblEWL_TC">
				  <xsl:for-each select="/dsSchema/tblCaptions">
					  <xsl:if test ="Field_ID = 'lblEWL_TC'">
						  <xsl:value-of select="FIELD_CAPTION"/>
					  </xsl:if>
				  </xsl:for-each>
			  </td>
              <td style = "width:15%" id="lblReservoirVolume_TC">
				  <xsl:for-each select="/dsSchema/tblCaptions">
					  <xsl:if test ="Field_ID = 'lblReservoirVolume_TC'">
						  <xsl:value-of select="FIELD_CAPTION"/>
					  </xsl:if>
				  </xsl:for-each>
			  </td>
              <td style = "width:5%"/>
            </tr>
          </table>
        </td>
      </tr>
      <tr style="height:5px">
        <td colspan = "3">
          <hr style="border-top: 1px solid #ccc;" color="brown" size="3"/>
        </td>
      </tr>
      <!-- FOR RACH ROW -->
      <xsl:for-each select="dsSchema/tblPatientVisit">
        <xsl:variable name="CommentOnly" select="CommentOnly"/>
        <xsl:if test="(position() &lt; 11 and Last10Visits = '1') or Last10Visits = '0'">
          <tr height="25">
            <xsl:if test="position() mod 2 = 0">
              <xsl:attribute name="class">row02</xsl:attribute>
            </xsl:if>
            <xsl:if test="position() mod 2 != 0">
              <xsl:attribute name="class">row01</xsl:attribute>
            </xsl:if>
            <xsl:if test="$CommentOnly = 1">
              <xsl:attribute name ="class">rowComment</xsl:attribute>
            </xsl:if>
            <td colspan = "3">
              <table style = "width : 100%"  cellpadding="0" cellspacing = "0" >
                <tr style = "text-align : Left; height : 25; font-size : small; font-family:Arial">
                  <td style = "width:15%">
                    <xsl:value-of select="DoctorInitial"/>
                  </td>
                  <td style = "width:15%; text-align :left">
                    <xsl:value-of select="strDateSeen"/>
                  </td>
                  <td style = "width:10%" >
                    <xsl:choose>
                      <xsl:when test ="VisitWeeksFlag = '0'">
                        <xsl:call-template name ="fnLoadNumber">
                          <xsl:with-param name="numValue" select ="Weeks"/>
                        </xsl:call-template>
                      </xsl:when>
                      <xsl:when test ="VisitWeeksFlag = '1'">
                        <xsl:call-template name ="fnLoadNumber">
                          <xsl:with-param name="numValue" select ="WeeksFromFirstVisit"/>
                        </xsl:call-template>
                      </xsl:when>
                      <xsl:when test ="VisitWeeksFlag = '3'">
                        <xsl:call-template name ="fnLoadNumber">
                          <xsl:with-param name="numValue" select ="WeeksFromZeroDate"/>
                        </xsl:call-template>
                      </xsl:when>
                      <xsl:when test ="VisitWeeksFlag = '4'">
                        <xsl:call-template name ="fnLoadNumber">
                          <xsl:with-param name="numValue" select ="WeeksFromOperationDate"/>
                        </xsl:call-template>
                      </xsl:when>
                    </xsl:choose>
                  </td>
                  <td style = "width:10%" >
                    <xsl:call-template name = "fnLoadNumber">
                      <xsl:with-param name ="numValue" select="Weight"></xsl:with-param>
                    </xsl:call-template>
                  </td>
                  <td style = "width:10%">
                    <xsl:choose>
                      <xsl:when test ="VisitWeeksFlag = '0'">
                        <xsl:call-template name ="fnLoadNumber">
                          <xsl:with-param name="numValue" select ="WeightLoss"/>
                        </xsl:call-template>
                        <!--<xsl:value-of select="round(WeightLoss)"/>-->
                      </xsl:when>
                      <xsl:when test ="VisitWeeksFlag = '1'">
                        <xsl:call-template name ="fnLoadNumber">
                          <xsl:with-param name="numValue" select ="WeightLossFromFirstVisit"/>
                        </xsl:call-template>
                      </xsl:when>
                      <xsl:when test ="VisitWeeksFlag = '3'">
                        <xsl:call-template name ="fnLoadNumber">
                          <xsl:with-param name="numValue" select ="WeightLossFromOperationDate"/>
                        </xsl:call-template>
                      </xsl:when>
                      <xsl:when test ="VisitWeeksFlag = '4'">
                        <xsl:call-template name ="fnLoadNumber">
                          <xsl:with-param name="numValue" select ="WeightLossFromOperationDate"/>
                        </xsl:call-template>
                      </xsl:when>
                    </xsl:choose>
                  </td>
                  <td style = "width:10%" >
                    <xsl:call-template name ="fnLoadNumber">
                      <xsl:with-param name="numValue" select ="BMI"/>
                    </xsl:call-template>
                  </td>
                  <td style = "width:10%" >
                    <xsl:choose>
                      <xsl:when test ="VisitWeeksFlag = '3'">
                        <xsl:call-template name ="fnLoadNumber">
                          <xsl:with-param name="numValue" select ="EWLOperationDate"/>
                        </xsl:call-template>
                      </xsl:when>
                      <xsl:when test ="VisitWeeksFlag = '4'">
                        <xsl:call-template name ="fnLoadNumber">
                          <xsl:with-param name="numValue" select ="EWLOperationDate"/>
                        </xsl:call-template>
                      </xsl:when>
                      <xsl:otherwise>
                        <xsl:call-template name ="fnLoadNumber">
                          <xsl:with-param name="numValue" select ="EWL"/>
                        </xsl:call-template>
                      </xsl:otherwise>
                    </xsl:choose>
                  </td>
                  <td style = "width:15%">
                    <xsl:if test ="ReservoirVolume != 0">
                      <xsl:value-of select="ReservoirVolume"/>
                    </xsl:if>
                  </td>
                  <td style = "width:5%"/>
                </tr>
                <xsl:if test="IncludeProgressNote = '1'">
                  <tr >
                    <td/>
                    <td colspan = "10" style="font-size:9pt">
                      <xsl:value-of select="Notes"/>
                    </td>
                  </tr>
                </xsl:if>
                  <tr >
                    <td/>
                    <td colspan = "10" style="font-size:9pt">
                      <xsl:value-of select="comorbidityNote" disable-output-escaping="yes"/>
                    </td>
                  </tr>
              </table>
            </td>

          </tr>

        </xsl:if>
      </xsl:for-each>
      <!--  END OF FOR EACH ROW -->
    </table>
  </body>
  </html>
</xsl:template>
  <xsl:template name = "fnLoadNumber">
    <xsl:param name ="numValue" ></xsl:param>

    <xsl:choose >
      <xsl:when test = "numValue=round($numValue)">
        <xsl:value-of select="round($numValue)"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="format-number($numValue, '###.#')"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name = "fnLoadNumberBlank">
    <xsl:param name ="numValue" ></xsl:param>

    <xsl:choose >
      <xsl:when test = "numValue=round($numValue)">
        <xsl:value-of select="round($numValue)"/>
      </xsl:when>
      <xsl:when test = "round($numValue) = 0">
        <xsl:value-of select="0"/>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="format-number($numValue, '###.#')"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template> 

<xsl:template name="break">
	<xsl:param name="text" select="."/>
	<xsl:choose>
		<xsl:when test="contains($text, '&#xa;')">
			<xsl:value-of select="substring-before($text, '&#xa;')"/>
			<br/>
			<xsl:call-template name="break">
				<xsl:with-param name="text" select="substring-after($text,'&#xa;')"/>
			</xsl:call-template>
		</xsl:when>
		<xsl:otherwise>
			<xsl:value-of select="$text"/>
		</xsl:otherwise>
	</xsl:choose>
</xsl:template>

</xsl:stylesheet>

