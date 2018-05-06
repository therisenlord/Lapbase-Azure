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
          <xsl:variable name="CommentOnly" select="CommentOnly"/>
          <xsl:variable name="EventType" select="EventType"/>
          <xsl:variable name="LatestNextVisit" select="latestNextVisit"/>
        <tr>
          <td>
            <xsl:if test='position() mod 2 = 1'>
              <xsl:attribute name="class">row01</xsl:attribute>
            </xsl:if>
            <xsl:if test='position() mod 2 != 1'>
              <xsl:attribute name="class">row02</xsl:attribute>
            </xsl:if>
            <xsl:if test='$CommentOnly = 1'>
              <xsl:attribute name ="class">rowComment</xsl:attribute>
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
                  <xsl:attribute name ="onclick">
                    javascript:VisitRowCheck_onclick(<xsl:value-of select="ConsultId"/>,<xsl:value-of select="CommentOnly"/>);
                  </xsl:attribute>
                  <xsl:value-of select="tempDateSeen"/>
                </td>
                <td style="width:5%">
                  <xsl:attribute name="id">Weeks_<xsl:value-of select="position()"/></xsl:attribute>
                  <xsl:attribute name ="onclick">
                    javascript:VisitRowCheck_onclick(<xsl:value-of select="ConsultId"/>,<xsl:value-of select="CommentOnly"/>);
                  </xsl:attribute>
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
                <td style="width:5%">
                  <xsl:attribute name="id">Weight_<xsl:value-of select="position()"/></xsl:attribute>
                  <xsl:attribute name ="onclick">
                    javascript:VisitRowCheck_onclick(<xsl:value-of select="ConsultId"/>,<xsl:value-of select="CommentOnly"/>);
                  </xsl:attribute>
					<xsl:call-template name ="fnLoadNumber">
						<xsl:with-param name="numValue" select ="Weight"/>
					</xsl:call-template>
				</td>
                <td style="width:5%" name ="ReservoirVolume">
                  <xsl:attribute name="id">ReservoirVolume_<xsl:value-of select="position()"/></xsl:attribute>
                  <xsl:attribute name ="onclick">
                    javascript:VisitRowCheck_onclick(<xsl:value-of select="ConsultId"/>,<xsl:value-of select="CommentOnly"/>);
                  </xsl:attribute>
                  <xsl:value-of select="ReservoirVolume"/>
                </td>
                <td style="width:5%">
                  <xsl:attribute name="id">
                    WeightLoss_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:attribute name ="onclick">
                    javascript:VisitRowCheck_onclick(<xsl:value-of select="ConsultId"/>,<xsl:value-of select="CommentOnly"/>);
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
                  <xsl:attribute name ="onclick">
                    javascript:VisitRowCheck_onclick(<xsl:value-of select="ConsultId"/>,<xsl:value-of select="CommentOnly"/>);
                  </xsl:attribute>
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
                  <xsl:attribute name ="onclick">
                    javascript:VisitRowCheck_onclick(<xsl:value-of select="ConsultId"/>,<xsl:value-of select="CommentOnly"/>);
                  </xsl:attribute>
					<xsl:call-template name ="fnLoadNumber">
						<xsl:with-param name="numValue" select ="BMI"/>
					</xsl:call-template>
                </td>
                <td style="width:5%">
                  <xsl:attribute name="id">EWL_<xsl:value-of select="position()"/></xsl:attribute>
                  <xsl:attribute name ="onclick">
                    javascript:VisitRowCheck_onclick(<xsl:value-of select="ConsultId"/>,<xsl:value-of select="CommentOnly"/>);
                  </xsl:attribute>
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
              javascript:VisitRowCheck_onclick(<xsl:value-of select="ConsultId"/>,<xsl:value-of select="CommentOnly"/>);
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
              javascript:VisitRowCheck_onclick(<xsl:value-of select="ConsultId"/>,<xsl:value-of select="CommentOnly"/>);
            </xsl:attribute>
					  <xsl:value-of select='translate(DoctorName, "`", "&apos;")'/>
					<input type='text' style='display:none'>
						<xsl:attribute name='id'>txtDoctorId_<xsl:value-of select="ConsultId"/></xsl:attribute>
						<xsl:attribute name="value"><xsl:value-of select='DoctorID'/></xsl:attribute>
					</input>
                </td>
                <td style="width:7%">
                  <xsl:attribute name ="onclick">
                    javascript:VisitRowCheck_onclick(<xsl:value-of select="ConsultId"/>,<xsl:value-of select="CommentOnly"/>);
                  </xsl:attribute>
                  <xsl:if test ="position() = 1">
                    <xsl:attribute name="id">tempDateNextVisit_<xsl:value-of select="position()"/></xsl:attribute>
                  </xsl:if>
                  <xsl:if test ="position() > 1">
                  </xsl:if>
                  <xsl:if test ="$LatestNextVisit = '1'">
                    <xsl:value-of select="tempDateNextVisit"/>
                  </xsl:if>
                </td>
                <td style="width:20%" >
                  <xsl:attribute name="name">tdDocument</xsl:attribute>
                  <xsl:attribute name ="onclick">
                    javascript:VisitRowCheck_onclick(<xsl:value-of select="ConsultId"/>,<xsl:value-of select="CommentOnly"/>);
                  </xsl:attribute>
                  <xsl:attribute name="id">tdDocument_<xsl:value-of select="ConsultId"/></xsl:attribute>
                  <select style="width:100%">
                    <xsl:attribute name="id">cmbDocument_<xsl:value-of select="ConsultId"/></xsl:attribute>
                    <xsl:attribute name="name">cmbDocument</xsl:attribute>
                    <xsl:attribute name="onchange">javascript:cmbDocument_onchange(this);</xsl:attribute>
                    <xsl:for-each select="/dsSchema/tblDocuments">
                      <xsl:if test="$ConsultID = EventID and $EventType = EventLink">
                        <option>
                          <xsl:attribute name="value">
                            <xsl:value-of select="tblPatientDocumentsID"/>
                          </xsl:attribute>
                          <xsl:attribute name="id">
                            <xsl:value-of select="DocumentFile"/>
                          </xsl:attribute>
                          <xsl:value-of select="Consult_DocumentName"/>
                        </option>
                      </xsl:if>
                    </xsl:for-each>
                  </select>
                </td>


                <td style="width:0%; display:none"><xsl:attribute name="id">resultAdd_<xsl:value-of select="position()"/></xsl:attribute><xsl:call-template name ="fnLoadNumber"><xsl:with-param name="numValue" select ="PulseRate"/></xsl:call-template>*^*<xsl:call-template name ="fnLoadNumber"><xsl:with-param name="numValue" select ="RespiratoryRate"/></xsl:call-template>*^*<xsl:call-template name ="fnLoadNumber"><xsl:with-param name="numValue" select ="BloodPressureUpper"/></xsl:call-template>*^*<xsl:call-template name ="fnLoadNumber"><xsl:with-param name="numValue" select ="BloodPressureLower"/></xsl:call-template>*^*<xsl:call-template name ="fnLoadNumber"><xsl:with-param name="numValue" select ="Neck"/></xsl:call-template>*^*<xsl:call-template name ="fnLoadNumber"><xsl:with-param name="numValue" select ="Waist"/></xsl:call-template>*^*<xsl:call-template name ="fnLoadNumber"><xsl:with-param name="numValue" select ="Hip"/></xsl:call-template>*^*<xsl:value-of select="GeneralReview"/>*^*<xsl:value-of select="CardiovascularReview"/>*^*<xsl:value-of select="RespiratoryReview"/>*^*<xsl:value-of select="GastroReview"/>*^*<xsl:value-of select="GenitoReview"/>*^*<xsl:value-of select="ExtremitiesReview"/>*^*<xsl:value-of select="NeurologicalReview"/>*^*<xsl:value-of select="MusculoskeletalReview"/>*^*<xsl:value-of select="SkinReview"/>*^*<xsl:value-of select="PsychiatricReview"/>*^*<xsl:value-of select="EndocrineReview"/>*^*<xsl:value-of select="HematologicReview"/>*^*<xsl:value-of select="ENTReview"/>*^*<xsl:value-of select="EyesReview"/>*^*<xsl:value-of select="PFSHReview"/>*^*<xsl:value-of select="MedicationsReview"/>*^*<xsl:value-of select="SatietyStaging"/>*^*<xsl:value-of select="ChiefComplaint"/>*^*<xsl:value-of select="LapbandAdjustment"/>*^*<xsl:value-of select="MedicalProviderID"/>*^*<xsl:value-of select="AdjConsent"/>*^*<xsl:value-of select="AdjAntiseptic"/>*^*<xsl:value-of select="AdjAnesthesia"/>*^*<xsl:value-of select="AdjAnesthesiaVol"/>*^*<xsl:value-of select="AdjNeedle"/>*^*<xsl:value-of select="AdjVolume"/>*^*<xsl:value-of select="AdjInitialVol"/>*^*<xsl:value-of select="AdjAddVol"/>*^*<xsl:value-of select="AdjRemoveVol"/>*^*<xsl:value-of select="AdjTolerate"/>*^*<xsl:value-of select="LetterSent"/>*^*<xsl:choose><xsl:when test ="VisitWeeksFlag = '3'"><xsl:value-of select="tempZeroDate"/></xsl:when><xsl:when test ="VisitWeeksFlag = '4'"><xsl:value-of select="tempOperationDate"/></xsl:when><xsl:otherwise><xsl:value-of select="tempFirstVisitDate"/></xsl:otherwise></xsl:choose>*^*<xsl:choose><xsl:when test ="VisitWeeksFlag = '3'"><xsl:value-of select="DefOperationDateWeight"/></xsl:when><xsl:when test ="VisitWeeksFlag = '4'"><xsl:value-of select="DefOperationDateWeight"/></xsl:when><xsl:otherwise><xsl:value-of select="DefFirstWeight"/></xsl:otherwise></xsl:choose>*^*<xsl:value-of select="DefIdealWeight"/>*^*<xsl:value-of select="BMIHeight"/>*^*<xsl:value-of select="VisitWeeksFlag"/>*^*<xsl:value-of select="ImperialFlag"/>*^*<xsl:value-of select="DefWeight"/>*^*<xsl:value-of select="DefPrevWeight"/>*^*<xsl:value-of select="DateCreatedFormated"/>*^*<xsl:value-of select="SupportGroup"/>*^*<xsl:value-of select="AdjBarium"/>*^*<xsl:value-of select="AdjOmni"/>*^*<xsl:value-of select="ProgressReview"/>*^*<xsl:value-of select="AdjProtocol"/>*^*<xsl:value-of select="CommentOnly"/>*^*<xsl:value-of select="RegistryReview"/>*^*<xsl:value-of select="RegistrySleepApnea"/>*^*<xsl:value-of select="RegistryGerd"/>*^*<xsl:value-of select="RegistryHyperlipidemia"/>*^*<xsl:value-of select="RegistryDiabetes"/>*^*<xsl:value-of select="monthGroup"/>*^*<xsl:value-of select="RegistryDiabetesDetail"/>*^*<xsl:value-of select="RegistryTreatmentDiet"/>*^*<xsl:value-of select="RegistryTreatmentOral"/>*^*<xsl:value-of select="RegistryTreatmentInsulin"/>*^*<xsl:value-of select="RegistryTreatmentOther"/>*^*<xsl:value-of select="RegistryTreatmentCombination"/>*^*<xsl:value-of select="RegistryReoperation"/>*^*<xsl:value-of select="RegistryReoperationNote"/>*^*<xsl:value-of select="RegistrySEDetail"/>*^*<xsl:value-of select="RegistrySEList"/>*^*<xsl:value-of select="RegistrySENote"/></td>
                
<!--
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
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    CommentOnly_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="CommentOnly"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistryReview_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistryReview"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistrySleepApnea_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistrySleepApnea"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistryGerd_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistryGerd"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistryHyperlipidemia_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistryHyperlipidemia"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistryDiabetes_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistryDiabetes"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    MonthGroup_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="monthGroup"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistryDiabetesDetail_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistryDiabetesDetail"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistryTreatmentDiet_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistryTreatmentDiet"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistryTreatmentOral_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistryTreatmentOral"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistryTreatmentInsulin_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistryTreatmentInsulin"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistryTreatmentOther_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistryTreatmentOther"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistryTreatmentCombination_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistryTreatmentCombination"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistryReoperation_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistryReoperation"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistryReoperationNote_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistryReoperationNote"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistrySEDetail_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistrySEDetail"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistrySEList_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistrySEList"/>
                </td>
                <td style="width:0%; display:none">
                  <xsl:attribute name="id">
                    RegistrySENote_<xsl:value-of select="position()"/>
                  </xsl:attribute>
                  <xsl:value-of select="RegistrySENote"/>
                </td>
                -->
                </tr>
              <xsl:if test ="string-length(Notes) != 0">
                <tr>
                  <xsl:attribute name ="onclick">
                    javascript:VisitRowCheck_onclick(<xsl:value-of select="ConsultId"/>,<xsl:value-of select="CommentOnly"/>);
                  </xsl:attribute>
                  <td />
                  <td colspan="9">
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

