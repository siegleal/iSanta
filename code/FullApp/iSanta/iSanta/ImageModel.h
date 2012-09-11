//
//  ImageModel.h
//  iSanta
//
//  Created by Siegle, Andrew L on 9/11/12.
//
//

#import <Foundation/Foundation.h>

@interface ImageModel : NSObject


@property (nonatomic, strong) UIImage *circleImage;
@property (nonatomic, strong) UIImage *normalBlackImage;
@property (nonatomic, strong) UIImage *normalPinkImage;
@property (nonatomic, strong) UIImage *normalOrangeImage;
@property (nonatomic, strong) UIImage *editImage;
@property (nonatomic, strong) NSArray *animationArray;

@property (nonatomic, strong) UIImage *upImage;
@property (nonatomic, strong) UIImage *dnImage;
@property (nonatomic, strong) UIImage *lfImage;
@property (nonatomic, strong) UIImage *rtImage;

@end
