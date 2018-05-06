<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">

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
      width: 98%;
      page-break-after:always;
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
	<body >
		<div >
			<table class="printPage"  cellpadding="0" cellspacing="0">

        <tr>
          <td colspan="3">
            <xsl:if test ="dsSchema/tblReport/LogoPath != ''">
              <img>
                <xsl:attribute name="src">
                  <xsl:value-of select="dsSchema/tblReport/LogoPath"/>
                </xsl:attribute>
              </img>
            </xsl:if>
          </td>
        </tr>
        <tr>
          <td colspan="3">
            <xsl:attribute name="style">
            padding:<xsl:value-of select="dsSchema/tblReport/LetterheadMargin"/>cm 0 0 0
            </xsl:attribute>
            <xsl:value-of select="dsSchema/tblReport/CurrentDate"/>
          </td>
        </tr>
				<tr>
					<td colspan="3">
						<br/>
            <br/>
            <br/>
					</td>
				</tr>

				<tr>
					<td colspan="2" style="font-weight:bold;font-size:medium;padding-left:1cm;">
						<xsl:value-of select="dsSchema/RefDoctor/RefDr_Name"/>
					</td>
					<td style = "width:30%">
						<!--
          <xsl:if test ="dsSchema/RefDoctor2/RefDrId != '' ">
			  <label id="lblCopyTo">
				  <xsl:for-each select="/dsSchema/tblCaptions">
					  <xsl:if test ="Field_ID = 'lblCopyTo'">
						  <xsl:value-of select="FIELD_CAPTION"/>
					  </xsl:if>
				  </xsl:for-each>
			  </label>
          </xsl:if>-->
					</td>
				</tr>

				<tr>
					<td colspan="3" style="font-weight:bold;font-size:medium;">
            <xsl:attribute name="style">
              padding:0 0 0 1cm
            </xsl:attribute>
            <xsl:value-of select="dsSchema/RefDoctor/Address1"/>
          </td>
        </tr>
        <tr>
          <td colspan="3" style="font-weight:bold;font-size:medium;">
            <xsl:attribute name="style">
              padding:0 0 0 1cm
            </xsl:attribute>
            <xsl:value-of select="dsSchema/RefDoctor/Address2"/>
          </td>
        </tr>
        <tr>
          <td colspan="3" style="font-weight:bold;font-size:medium;">
            <xsl:attribute name="style">
              padding:0 0 0 1cm
            </xsl:attribute>
            <xsl:value-of select="dsSchema/RefDoctor/Address3"/>
          </td>
        </tr>

        <!--
      <tr>
        <td colspan="2">
          <xsl:if test ="dsSchema/RefDoctor2/RefDrId != ''">
            <xsl:value-of select="dsSchema/RefDoctor2/Address1"/> &#160;&#160;
            <xsl:value-of select="dsSchema/RefDoctor2/Address2"/> &#160;&#160;
            <xsl:value-of select="dsSchema/RefDoctor2/Address3"/> &#160;&#160;
          </xsl:if>
        </td>
        <td >
          <xsl:if test ="dsSchema/RefDoctor2/RefDrId != ''">
            <xsl:value-of select="dsSchema/RefDoctor2/RefDr_Name"/>
          </xsl:if>
        </td>
      </tr>

      <tr>
        <td colspan="2">
          <xsl:if test ="dsSchema/RefDoctor3/RefDrId != ''">
            <xsl:value-of select="dsSchema/RefDoctor3/Address1"/> &#160;&#160;
            <xsl:value-of select="dsSchema/RefDoctor3/Address2"/> &#160;&#160;
            <xsl:value-of select="dsSchema/RefDoctor3/Address3"/> &#160;&#160;
          </xsl:if>
        </td>
        <td >
          <xsl:if test ="dsSchema/RefDoctor3/RefDrId != ''">
            <xsl:value-of select="dsSchema/RefDoctor3/RefDr_Name"/>
          </xsl:if>
        </td>
      </tr>
		-->
				<tr>
					<td colspan="3">
						<br/>
            <br/>
					</td>
				</tr>

				<tr>
					<td colspan = "3">
						<xsl:value-of select="dsSchema/RefDoctor/RefDr_Salutation"/>
					</td>
				</tr>

				<!--<tr>
					<td colspan = "3">
						<label id="lblReferredBy">
							<xsl:for-each select="/dsSchema/tblCaptions">
								<xsl:if test ="Field_ID = 'lblReferredBy'">
									<xsl:value-of select="FIELD_CAPTION"/>
								</xsl:if>
							</xsl:for-each>
						</label>
						<xsl:value-of select="dsSchema/RefDoctor1/RefDr_Name"/>
					</td>
				</tr>-->

				<tr>
					<td colspan="3">
						<br/>
					</td>
				</tr>

        <tr>
          <td colspan = "3">
            <table>
              <tr>
                <td>
                  <label id="lblRe">
                    <xsl:for-each select="/dsSchema/tblCaptions">
                      <xsl:if test ="Field_ID = 'lblRe'">
                        <xsl:value-of select="FIELD_CAPTION"/>
                      </xsl:if>
                    </xsl:for-each>
                  </label>
                </td>
                <td style="font-weight:bold;">
                  <xsl:value-of select='translate(dsSchema/tblReport/Patname, "`", "&apos;")'/>
                </td>
              </tr>
              <tr>
                <td></td>
                <td>
                  <label id ="lblPatientID">
                    <xsl:for-each select="/dsSchema/tblCaptions">
                      <xsl:if test ="Field_ID = 'lblPatientID'">
                        <xsl:value-of select="FIELD_CAPTION"/>
                      </xsl:if>
                    </xsl:for-each>
                  </label>
                  <xsl:value-of select="dsSchema/tblReport/Patient_CustomID"/>
                </td>
              </tr>
              <tr>
                <td>
                </td>
                <td style="font-weight:bold;">
                  <xsl:value-of select="dsSchema/tblReport/Street"/>
                </td>
              </tr>
              <tr>
                <td>
                </td>
                <td style="font-weight:bold;">
                  <xsl:value-of select="dsSchema/tblReport/SuburbPostcode"/>
                </td>
              </tr>
              <tr>
                <td>
                </td>
                <td style="font-weight:bold;">
                DOB: <xsl:value-of select="dsSchema/tblReport/Birthdate"/>
                </td>
              </tr>
            </table>
          </td>
        </tr>

        <tr>
          <td colspan="3">
            <br/>
            <br/>
          </td>
        </tr>
        <tr style="display:none">
					<td colspan="3">
						<xsl:value-of select='translate(dsSchema/tblReport/FName, "`", "&apos;")'/>
						<label id="lblWasRecently">
							<xsl:for-each select="/dsSchema/tblCaptions">
								<xsl:if test ="Field_ID = 'lblWasRecently'">
									<xsl:value-of select="FIELD_CAPTION"/>
								</xsl:if>
							</xsl:for-each>
						</label>
					</td>
				</tr>

        <tr>
          <td colspan = "3" >
            <br />
            <xsl:value-of disable-output-escaping="yes" select="dsSchema/tblReport/FollowUpNotes"/>
          </td>
        </tr>
        
        <tr>
          <td colspan="3">
            <br/>
            <br/>
          </td>
        </tr>
        <tr>
          <td colspan="3" style="font-size : large; font-weight:bold;">
            <xsl:value-of select="dsSchema/tblReport/DoctorName"/>
          </td>
        </tr>
        <tr>
          <td colspan = "3">
            <xsl:value-of select="dsSchema/tblReport/Speciality"/>
          </td>
        </tr>
        <tr>
          <td colspan ="3">
            <xsl:value-of select="dsSchema/tblReport/Degrees"/>
          </td>
        </tr>
        <tr>
          <td colspan="3">
            <br/>
          </td>
        </tr>
        
				<tr>
					<td colspan="3">
						<hr />
					</td>
				</tr>

				<tr>
					<td colspan = "3" >
						<h3 id ="lblInitialAssessment">
							<xsl:for-each select="/dsSchema/tblCaptions">
								<xsl:if test ="Field_ID = 'lblInitialAssessment'">
									<xsl:value-of select="FIELD_CAPTION"/>
								</xsl:if>
							</xsl:for-each>
						</h3>
					</td>
				</tr>

				<tr>
					<td colspan="3">
						<table style="width:50%" border="0" cellpadding="0" cellspacing="0">
							<tr style="height:20px">
								<td style="width:12%;" id="lblHeight">
									<xsl:for-each select="/dsSchema/tblCaptions">
										<xsl:if test ="Field_ID = 'lblHeight'">
											<xsl:value-of select="FIELD_CAPTION"/>
										</xsl:if>
									</xsl:for-each>
								</td>
								<td style="width:8%" >
									<xsl:value-of select="round(dsSchema/tblReport/Height)"/>
								</td>
								<td style="width:7%" >
									<xsl:value-of select="dsSchema/tblReport/HeightMeasurment"/>
								</td>
								<td style="width:20%" id="lblIdealWeight">
									<xsl:for-each select="/dsSchema/tblCaptions">
										<xsl:if test ="Field_ID = 'lblIdealWeight'">
											<xsl:value-of select="FIELD_CAPTION"/>
										</xsl:if>
									</xsl:for-each>
								</td>
								<td style="width:8%" >
									<!--<xsl:value-of select="round(dsSchema/tblReport/IdealWeight)"/>-->
									<xsl:call-template name = "fnLoadNumber">
										<xsl:with-param name ="numValue" select="dsSchema/tblReport/IdealWeight"/>
									</xsl:call-template>
								</td>
								<td style="width:7%" >
									<xsl:value-of select="dsSchema/tblReport/WeightMeasurment"/>
								</td>
							</tr>

							<tr style="height:20px">
								<td id="lblWeight">
									<xsl:for-each select="/dsSchema/tblCaptions">
										<xsl:if test ="Field_ID = 'lblWeight'">
											<xsl:value-of select="FIELD_CAPTION"/>
										</xsl:if>
									</xsl:for-each>
								</td>
								<td>
									<!--<xsl:value-of select="round(dsSchema/tblReport/StartWeight)"/>-->
									<xsl:call-template name = "fnLoadNumber">
										<xsl:with-param name ="numValue" select="dsSchema/tblReport/StartWeight"/>
									</xsl:call-template>
								</td>
								<td>
									<xsl:value-of select="dsSchema/tblReport/WeightMeasurment"/>
								</td>
								<td id="lblTargetWeight">
									<xsl:for-each select="/dsSchema/tblCaptions">
										<xsl:if test ="Field_ID = 'lblTargetWeight'">
											<xsl:value-of select="FIELD_CAPTION"/>
										</xsl:if>
									</xsl:for-each>
								</td>
								<td>
									<!--<xsl:value-of select="round(dsSchema/tblReport/TargetWeight)"/>-->
									<xsl:call-template name = "fnLoadNumber">
										<xsl:with-param name ="numValue" select="dsSchema/tblReport/TargetWeight"/>
									</xsl:call-template>
								</td>
								<td>
									<xsl:value-of select="dsSchema/tblReport/WeightMeasurment"/>
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
									<!--<xsl:value-of select="dsSchema/tblReport/BMI"/>-->
									<xsl:call-template name = "fnLoadNumber">
										<xsl:with-param name ="numValue" select="dsSchema/tblReport/BMI"/>
									</xsl:call-template>
								</td>
								<td></td>
								<td id="lblAboveIdealWeight">
									<!--<xsl:for-each select="/dsSchema/tblCaptions">
										<xsl:if test ="Field_ID = 'lblAboveIdealWeight'">
											<xsl:value-of select="FIELD_CAPTION"/>
										</xsl:if>
									</xsl:for-each>-->
								</td>
								<td>
									<!--<xsl:value-of select="dsSchema/tblReport/ExcessWeight"/> -->
								</td>
								<td />
							</tr>
							<tr style="height:20px">
								<td id="lblOperationDate">
									<xsl:for-each select="/dsSchema/tblCaptions">
										<xsl:if test ="Field_ID = 'lblOperationDate'">
											<xsl:value-of select="FIELD_CAPTION"/>
										</xsl:if>
									</xsl:for-each>
								</td>
								<td colspan="2">
									<xsl:value-of select="dsSchema/tblReport/LapBandDate"/>
								</td>
							</tr>
						</table>
					</td>
				</tr>

				<tr>
					<td colspan="3">
						<hr />
					</td>
				</tr>

				<xsl:if test="dsSchema/tblReport/IncludePatientNote = '1'">
					<tr height="25">
						<td colspan = "3" >
							<h3 id="lblPatientNotes">
								<xsl:for-each select="/dsSchema/tblCaptions">
									<xsl:if test ="Field_ID = 'lblPatientNotes'">
										<xsl:value-of select="FIELD_CAPTION"/>
									</xsl:if>
								</xsl:for-each>
							</h3>
						</td>
					</tr>
					<tr>
						<td colspan="3">
							<!--<xsl:value-of select="dsSchema/tblReport/SignificantEvents"/>-->
							<xsl:call-template name = "break">
								<xsl:with-param name ="text" select="dsSchema/tblReport/SignificantEvents"/>
							</xsl:call-template>
						</td>
					</tr>
					<tr height="5x" style="height:5px">
						<td colspan = "3">
							<hr />
						</td>
					</tr>
				</xsl:if>
				<tr>
					<td colspan = "3">
						<h3 id="lblFollowUp">
							<xsl:for-each select="/dsSchema/tblCaptions">
								<xsl:if test ="Field_ID = 'lblFollowUp'">
									<xsl:value-of select="FIELD_CAPTION"/>
								</xsl:if>
							</xsl:for-each>
						</h3>
					</td>
				</tr>
				<tr>
					<td colspan = "3">
						<table style="width:100%" border="0">
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
								<td style = "width:10%" >
									<label id="lblCurrent_TC">
										<xsl:for-each select="/dsSchema/tblCaptions">
											<xsl:if test ="Field_ID = 'lblCurrent_TC'">
												<xsl:value-of select="FIELD_CAPTION"/>
											</xsl:if>
										</xsl:for-each>
									</label> (<xsl:value-of select="dsSchema/tblReport/WeightMeasurment"/>)
								</td>
								<td style = "width:10%">
									<label id="lblLoss_TC">
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
                  </label> (<xsl:value-of select="dsSchema/tblReport/WeightMeasurment"/>)
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
								<td/>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td colspan="3">
						<hr style="border-top: 1px solid #ccc;" color="brown" size="3"/>
					</td>
				</tr>
				<xsl:if test="dsSchema/tblReport/CurrentVisitOnly = '1'">
					<tr>
						<td colspan="3">
							<table border="0"  style="width:100%">
								<tr>
									<td style = "width:15%" >
										<xsl:value-of select="dsSchema/tblPatientVisit/DoctorName"/>
									</td>
									<td style = "width:15%">
										<xsl:value-of select="dsSchema/tblPatientVisit/strDateSeen"/>
									</td>
									<td style = "width:10%">
										<xsl:if test = "dsSchema/tblPatientVisit/Weeks != 0">
											<xsl:value-of select="dsSchema/tblPatientVisit/Weeks"/>
										</xsl:if>
									</td>
									<td style = "width:10%">
										<xsl:call-template name = "fnLoadNumber">
											<xsl:with-param name ="numValue" select="dsSchema/tblPatientVisit/Weight"/>
										</xsl:call-template>
									</td>
									<td style = "width:10%">
										<xsl:call-template name = "fnLoadNumber">
											<xsl:with-param name ="numValue" select="dsSchema/tblPatientVisit/WeightLoss"/>
										</xsl:call-template>
									</td>
									<td style = "width:10%">
										
										<xsl:call-template name = "fnLoadNumber">
											<xsl:with-param name ="numValue" select="dsSchema/tblPatientVisit/BMI"/>
										</xsl:call-template>
									</td>
									<td style = "width:10%">
										<xsl:call-template name = "fnLoadNumber">
											<xsl:with-param name ="numValue" select="dsSchema/tblPatientVisit/EWL"/>
										</xsl:call-template>
									</td>
									<td style = "width:15%">
										<xsl:value-of select="dsSchema/tblPatientVisit/ReservoirVolume"/>
									</td>
									<td/>
								</tr>
								<xsl:if test="dsSchema/tblPatientVisit/IncludeProgressNote = '1'">
									<tr>
										<td />
										<td colspan = "7">
											<xsl:value-of select="dsSchema/tblPatientVisit/Notes"/>
										</td>
									</tr>
								</xsl:if>
							</table>
						</td>
					</tr>
				</xsl:if>
				<xsl:if test="dsSchema/tblReport/CurrentVisitOnly != '1'">
					<xsl:for-each select="dsSchema/tblPatientVisit">

            <xsl:variable name="CommentOnly" select="CommentOnly"/>
            <xsl:if test="(position() &lt; 11 and Last10Visits = '1') or Last10Visits = '0'">
						  <tr>
							  <xsl:if test="position() mod 2 = 0">
								  <xsl:attribute name="class">row01</xsl:attribute>
							  </xsl:if>
							  <xsl:if test="position() mod 2 != 0">
								  <xsl:attribute name="class">row02</xsl:attribute>
							  </xsl:if>
                <xsl:if test="$CommentOnly = 1">
                  <xsl:attribute name ="class">rowComment</xsl:attribute>
                </xsl:if>
							  <td colspan="3">
								  <table border ="0" style="width:100%">
									  <tr>
										  <td style = "width:15%">
											  <xsl:value-of select="DoctorName"/>
										  </td>
										  <td style = "width:15%">
											  <xsl:value-of select="strDateSeen"/>
										  </td>
										  <td style = "width:10%">
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
                      <td style = "width:10%">
											  <xsl:call-template name = "fnLoadNumber">
												  <xsl:with-param name ="numValue" select="Weight"/>
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
										  <td style = "width:10%">
                        <xsl:call-template name ="fnLoadNumber">
                          <xsl:with-param name="numValue" select ="BMI"/>
                        </xsl:call-template>
										  </td>
										  <td style = "width:10%">
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
										  <td/>
									  </tr>
									  <xsl:if test="IncludeProgressNote = '1'">
										  <tr>
											  <td />
											  <td colspan = "7">
												  <xsl:value-of select="Notes"/>
											  </td>
										  </tr>
									  </xsl:if>
                        <tr >
                          <td/>
                          <td colspan = "7" style="font-size:9pt">
                            <xsl:value-of select="comorbidityNote" disable-output-escaping="yes"/>
                          </td>
                        </tr>
								  </table>
							  </td>
						  </tr>
              
            </xsl:if>
					</xsl:for-each>
				</xsl:if>
			</table>
		</div>
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

