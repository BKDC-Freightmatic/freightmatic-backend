import {
  Controller,
  Post,
  UseInterceptors,
  UploadedFiles,
} from '@nestjs/common';
import { FilesInterceptor } from '@nestjs/platform-express';
import { ApiTags, ApiOperation, ApiConsumes, ApiBearerAuth } from '@nestjs/swagger';
import { FilesService } from './files.service';

@ApiTags('files')
@ApiBearerAuth()
@Controller('v1/files')
export class FilesController {
  constructor(private readonly filesService: FilesService) {}

  @Post('upload')
  @UseInterceptors(FilesInterceptor('files'))
  @ApiConsumes('multipart/form-data')
  @ApiOperation({ summary: 'Upload file(s)' })
  async uploadFiles(@UploadedFiles() files: Express.Multer.File[]) {
    return this.filesService.uploadFiles(files);
  }
}
