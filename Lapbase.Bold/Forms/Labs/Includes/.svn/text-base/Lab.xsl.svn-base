<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt">

<xsl:template match="/">
    <html>
    <body>
      <table style="width:100%" cellpadding = "0" cellspacing = "0" border="0" id ="tblVisitXSLT" class="testNameTable">
        <xsl:for-each select="dsSchema/tblConsultFU1_ProgressNotes">
          <xsl:variable name="ConsultID" select="ConsultId"/>
        <tr>
          <td>
            <xsl:if test='position() mod 2 = 1'>
              <xsl:attribute name="class">row01</xsl:attribute>
            </xsl:if>
            <xsl:if test='position() mod 2 != 1'>
              <xsl:attribute name="class">row02</xsl:attribute>
            </xsl:if>
			  
            <table style="width:100%" cellpadding = "0" cellspacing = "0" border="0">
              <xsl:attribute name="id">tblVisitRow_<xsl:value-of select="ConsultId"/></xsl:attribute>
              <!--<xsl:attribute name="onmouseover">javascript:RowTable_onmouseover(this.id);</xsl:attribute>
              <xsl:attribute name="onmouseout">javascript:RowTable_onmouseout(this.id);</xsl:attribute>-->
				<xsl:attribute name="onmouseover">
					javascript:this.style.cursor='pointer';
				</xsl:attribute>
				<xsl:attribute name="onmouseout">
					javascript:this.style.cursor='';
				</xsl:attribute>
              
              <tr>
                <td style="width:10%">
                  <xsl:attribute name="id">tempDateSeen_<xsl:value-of select="position()"/></xsl:attribute>
                  <xsl:attribute name ="onclick">javascript:VisitRow_onclick(<xsl:value-of select="ConsultId"/>);</xsl:attribute>
                  <xsl:value-of select="tempDateSeen"/>
                </td>
                <td style="width:5%">
                  <xsl:attribute name="id">Weeks_<xsl:value-of select="position()"/></xsl:attribute>
                  <xsl:attribute name ="onclick">javascript:VisitRow_onclick(<xsl:value-of select="ConsultId"/>);</xsl:attribute>
					        <xsl:choose>
						        <xsl:when test ="VisitWeeksFlag = '0'">
							        <xsl:value-of select="Weeks"/>
						        </xsl:when>
                    <xsl:when test ="VisitWeeksFlag = '1'">
                      <xsl:value-of select="WeeksFromFirstVisit"/>
                    </xsl:when>
                    <xsl:when test ="VisitWeeksFlag = '3'">
                      <xsl:value-of select="WeeksFromZeroDate"/>
                    </xsl:when>
                    <xsl:when test ="VisitWeeksFlag = '4'">
                      <xsl:value-of select="WeeksFromOperationDate"/>
                    </xsl:when>
					        </xsl:choose>
                </td>
                <td style="width:5%">
                  <xsl:attribute name="id">Weight_<xsl:value-of select="position()"/></xsl:attribute>
                  <xsl:attribute name ="onclick">javascript:VisitRow_onclick(<xsl:value-of select="ConsultId"/>);</xsl:attribute>
					<xsl:call-template name ="fnLoadNumber">
						<xsl:with-param name="numValue" select ="Weight"/>
					</xsl:call-template>
				</td>
                <td style="width:5%" name ="ReservoirVolume">
                  <xsl:attribute name="id">ReservoirVolume_<xsl:value-of select="position()"/></xsl:attribute>
                  <xsl:attribute name ="onclick">javascript:VisitRow_onclick(<xsl:value-of select="ConsultId"/>);</xsl:attribute>
                  <xsl:value-of select="ReservoirVolume"/>
                </td>
                <td style="width:5%">
                  <xsl:attribute name="id">
                    WeightLoss_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:attribute name ="onclick">
                    javascript:VisitRow_onclick(<xsl:value-of select="ConsultId"/>);
                  </xsl:attribute>
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
                    <!--<xsl:otherwise>
                      <xsl:call-template name ="fnLoadNumber">
                        <xsl:with-param name="numValue" select ="WeightLossFromFirstVisit"/>
                      </xsl:call-template>
                      <xsl:value-of select="round(WeightLossFromFirstVisit)"/>
                    </xsl:otherwise>-->
                  </xsl:choose>
                </td>
                <td style="width:10%; display:none">
                  <xsl:attribute name="id">WeightLossPerWeek_<xsl:value-of select="position()"/></xsl:attribute>
                  <xsl:attribute name ="onclick">javascript:VisitRow_onclick(<xsl:value-of select="ConsultId"/>);</xsl:attribute>
					        <xsl:choose>
						        <xsl:when test ="VisitWeeksFlag = '0'">
							        <xsl:call-template name ="fnLoadNumber">
								        <xsl:with-param name="numValue" select ="WeightLossPerWeek"/>
							        </xsl:call-template>
							        <!--<xsl:value-of select="round(WeightLossPerWeek)"/>-->
						        </xsl:when>
                    <xsl:when test ="VisitWeeksFlag = '1'">
                      <xsl:call-template name ="fnLoadNumber">
                        <xsl:with-param name="numValue" select ="WeightLossPerWeekFromFirstVisit"/>
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
                    
						        <!--<xsl:otherwise>
							        <xsl:call-template name ="fnLoadNumber">
								        <xsl:with-param name="numValue" select ="WeightLossPerWeekFromFirstVisit"/>
							        </xsl:call-template>
						        </xsl:otherwise>-->
					        </xsl:choose>
                </td>
                <td style="width:5%">
                  <xsl:attribute name="id">BMI_<xsl:value-of select="position()"/></xsl:attribute>
                  <xsl:attribute name ="onclick">javascript:VisitRow_onclick(<xsl:value-of select="ConsultId"/>);</xsl:attribute>
					<xsl:call-template name ="fnLoadNumber">
						<xsl:with-param name="numValue" select ="BMI"/>
					</xsl:call-template>
                </td>
                <td style="width:5%">
                  <xsl:attribute name="id">EWL_<xsl:value-of select="position()"/></xsl:attribute>
                  <xsl:attribute name ="onclick">javascript:VisitRow_onclick(<xsl:value-of select="ConsultId"/>);</xsl:attribute>
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
				  <td style="width:5%; display:none">
					  <xsl:attribute name="id">
						  BloodPressure_<xsl:value-of select="position()"/>
					  </xsl:attribute>
					  <xsl:attribute name ="onclick">
						  javascript:VisitRow_onclick(<xsl:value-of select="ConsultId"/>);
					  </xsl:attribute>
					  <xsl:call-template name ="fnLoadNumber">
						  <xsl:with-param name="numValue" select ="BloodPressure"/>
					  </xsl:call-template>
				  </td>
				  <td style="width:12%">
					  <xsl:attribute name="id">
						  DoctorName_<xsl:value-of select="position()"/>
					  </xsl:attribute>
					  <xsl:attribute name ="onclick">
						  javascript:VisitRow_onclick(<xsl:value-of select="ConsultId"/>);
					  </xsl:attribute>
					  <xsl:value-of select='translate(DoctorName, "`", "&apos;")'/>
					<input type='text' style='display:none'>
						<xsl:attribute name='id'>txtDoctorId_<xsl:value-of select="ConsultId"/></xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select='DoctorID'/></xsl:attribute>
					</input>
                </td>
                <td style="width:7%">
                  <xsl:if test ="position() = 1">
                    <xsl:attribute name="id">tempDateNextVisit_<xsl:value-of select="position()"/></xsl:attribute>
                    <xsl:attribute name ="onclick">javascript:VisitRow_onclick(<xsl:value-of select="ConsultId"/>);</xsl:attribute>
                    <xsl:value-of select="tempDateNextVisit"/>
                  </xsl:if>
                  <xsl:if test ="position() > 1">
                    <xsl:attribute name ="onclick">javascript:VisitRow_onclick(<xsl:value-of select="ConsultId"/>);</xsl:attribute>
                  </xsl:if>
                </td>
                <td style="width:20%" >
                  <xsl:attribute name="name">tdDocument</xsl:attribute>
                  <xsl:attribute name="id">tdDocument_<xsl:value-of select="ConsultId"/></xsl:attribute>
                  <select style="width:100%">
                    <xsl:attribute name="id">cmbDocument_<xsl:value-of select="ConsultId"/></xsl:attribute>
                    <xsl:attribute name="name">cmbDocument</xsl:attribute>
                    <xsl:attribute name="onchange">javascript:cmbDocument_onchange(this);</xsl:attribute>
                    <xsl:for-each select="/dsSchema/tblDocuments">
                      <xsl:if test="$ConsultID = EventID">
                        <option>
                          <xsl:attribute name="value">
                            <xsl:value-of select="tblPatientDocumentsID"/>
                          </xsl:attribute>
                          <xsl:value-of select="Consult_DocumentName"/>
                        </option>
                      </xsl:if>
                    </xsl:for-each>
                  </select>
                </td>

                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    PR_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:call-template name ="fnLoadNumber">
                    <xsl:with-param name="numValue" select ="PulseRate"/>
                  </xsl:call-template>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RR_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:call-template name ="fnLoadNumber">
                    <xsl:with-param name="numValue" select ="RespiratoryRate"/>
                  </xsl:call-template>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    BP1_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:call-template name ="fnLoadNumber">
                    <xsl:with-param name="numValue" select ="BloodPressureUpper"/>
                  </xsl:call-template>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    BP2_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:call-template name ="fnLoadNumber">
                    <xsl:with-param name="numValue" select ="BloodPressureLower"/>
                  </xsl:call-template>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Neck_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:call-template name ="fnLoadNumber">
                    <xsl:with-param name="numValue" select ="Neck"/>
                  </xsl:call-template>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Waist_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:call-template name ="fnLoadNumber">
                    <xsl:with-param name="numValue" select ="Waist"/>
                  </xsl:call-template>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Hip_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:call-template name ="fnLoadNumber">
                    <xsl:with-param name="numValue" select ="Hip"/>
                  </xsl:call-template>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    General_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="GeneralReview"/>
                </td>                
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Cardio_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="CardiovascularReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Resp_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RespiratoryReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Gastro_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="GastroReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Genito_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="GenitoReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Extr_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="ExtremitiesReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Neuro_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="NeurologicalReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Musculo_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="MusculoskeletalReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Skin_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="SkinReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Psych_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="PsychiatricReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Endo_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="EndocrineReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Hema_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="HematologicReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    ENT_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="ENTReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Eyes_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="EyesReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    PFSH_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="PFSHReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Meds_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="MedicationsReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    Satiety_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="SatietyStaging"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    ChiefComplaint_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="ChiefComplaint"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    LapbaseAdjustment_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="LapbandAdjustment"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    MedicalProviderID_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="MedicalProviderID"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    AdjConsent_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="AdjConsent"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    AdjAntiseptic_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="AdjAntiseptic"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    AdjAnesthesia_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="AdjAnesthesia"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    AdjAnesthesiaVol_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="AdjAnesthesiaVol"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    AdjNeedle_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="AdjNeedle"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    AdjVolume_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="AdjVolume"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    AdjInitialVol_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="AdjInitialVol"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    AdjAddVol_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="AdjAddVol"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    AdjRemoveVol_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="AdjRemoveVol"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    AdjTolerate_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="AdjTolerate"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    LetterSent_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="LetterSent"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    tempFirstVisitDate_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:choose>
                    <xsl:when test ="VisitWeeksFlag = '3'">
                      <xsl:value-of select="tempZeroDate"/>
                    </xsl:when>
                    <xsl:when test ="VisitWeeksFlag = '4'">
                      <xsl:value-of select="tempOperationDate"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="tempFirstVisitDate"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    DefFirstWeight_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:choose>
                    <xsl:when test ="VisitWeeksFlag = '3'">
                      <xsl:value-of select="DefOperationDateWeight"/>
                    </xsl:when>
                    <xsl:when test ="VisitWeeksFlag = '4'">
                      <xsl:value-of select="DefOperationDateWeight"/>
                    </xsl:when>
                    <xsl:otherwise>
                      <xsl:value-of select="DefFirstWeight"/>
                    </xsl:otherwise>
                  </xsl:choose>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    DefIdealWeight_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="DefIdealWeight"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    BMIHeight_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="BMIHeight"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    VisitWeeksFlag_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="VisitWeeksFlag"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    ImperialFlag_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="ImperialFlag"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    DefWeight_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="DefWeight"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    DefPrevWeight_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="DefPrevWeight"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    DateCreated_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="DateCreatedFormated"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    SupportGroup_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="SupportGroup"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    AdjBarium_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="AdjBarium"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    AdjOmni_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="AdjOmni"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    ProgressReview_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="ProgressReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    AdjProtocol_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="AdjProtocol"/>
                </td>
                
              </tr>
              <xsl:if test ="string-length(Notes) != 0">
                <tr>
                  <xsl:attribute name ="onclick">javascript:VisitRow_onclick(<xsl:value-of select="ConsultId"/>);</xsl:attribute>
                  <td />
                  <td colspan="10">
                    <xsl:attribute name="id">Notes_<xsl:value-of select="position()"/></xsl:attribute>
                    <span>
                      <!--<xsl:attribute name ="style">background-Color:#c6c7c2</xsl:attribute>-->
                      <xsl:value-of select="Notes"/>
                    </span>
                  </td>
                </tr>
              </xsl:if>
            </table>
          </td>
        </tr>
        </xsl:for-each>
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

</xsl:stylesheet> 

