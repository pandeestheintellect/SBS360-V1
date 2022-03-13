using Eng360Web.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eng360Web.Models.Service.Imp
{
    public interface IQualityDefectServices
    {
       
        int CreateQualityDefect(QualityDefectViewModel eng_qa_defect, string path, string halfPath);
        int SaveQualityDefect(QualityDefectViewModel eng_qa_defect, string path, string halfPath);
        int DeleteQualityDefect(int qualitydefectID);
        QualityDefectViewModel getQualityDefect(int qualitydefectID);
        List<QualityDefectViewModel> getAllQualityDefects();
        QualityDefectDetailViewModel getQualityDefectDetail(int? id);

        
        int deleteQualityDefectDetail(int? id);
        int deleteQualityDefectFile(int? id);
        int ApproveRejectQualityDefect(QualityDefectViewModel qualitydefect);
        List<QualityDefectDetailViewModel> getAllDefectDetails();
        QualityDefectCPAViewModel getQualityCPADetail(int? id);
        int AddEditCpa(QualityDefectCPAViewModel eng_qa_defect_cpa, string path, string halfPath);
        int deleteQualityCPAFile(int? id);

        int CreateQualityDefect(MobileQualityDefectViewModel eng_qa_defect, string path, string halfPath);
        int AddEditCpa(MobileQualityDefectCPAViewModel eng_qa_defect_cpa, string path, string halfPath);
    }
}