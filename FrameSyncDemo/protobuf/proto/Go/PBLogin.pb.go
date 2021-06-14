// Code generated by protoc-gen-go. DO NOT EDIT.
// source: PBLogin.proto

/*
Package PBLogin is a generated protocol buffer package.

It is generated from these files:
	PBLogin.proto

It has these top-level messages:
	TcpLogin
	TcpResponseLogin
*/
package PBLogin

import proto "github.com/golang/protobuf/proto"
import fmt "fmt"
import math "math"

// Reference imports to suppress errors if they are not otherwise used.
var _ = proto.Marshal
var _ = fmt.Errorf
var _ = math.Inf

// This is a compile-time assertion to ensure that this generated file
// is compatible with the proto package it is being compiled against.
// A compilation error at this line likely means your copy of the
// proto package needs to be updated.
const _ = proto.ProtoPackageIsVersion2 // please upgrade the proto package

type TcpLogin struct {
	Token            *string `protobuf:"bytes,1,req,name=token" json:"token,omitempty"`
	XXX_unrecognized []byte  `json:"-"`
}

func (m *TcpLogin) Reset()                    { *m = TcpLogin{} }
func (m *TcpLogin) String() string            { return proto.CompactTextString(m) }
func (*TcpLogin) ProtoMessage()               {}
func (*TcpLogin) Descriptor() ([]byte, []int) { return fileDescriptor0, []int{0} }

func (m *TcpLogin) GetToken() string {
	if m != nil && m.Token != nil {
		return *m.Token
	}
	return ""
}

type TcpResponseLogin struct {
	Result           *bool  `protobuf:"varint,1,req,name=result" json:"result,omitempty"`
	Uid              *int32 `protobuf:"varint,2,req,name=uid" json:"uid,omitempty"`
	UdpPort          *int32 `protobuf:"varint,3,req,name=udpPort" json:"udpPort,omitempty"`
	XXX_unrecognized []byte `json:"-"`
}

func (m *TcpResponseLogin) Reset()                    { *m = TcpResponseLogin{} }
func (m *TcpResponseLogin) String() string            { return proto.CompactTextString(m) }
func (*TcpResponseLogin) ProtoMessage()               {}
func (*TcpResponseLogin) Descriptor() ([]byte, []int) { return fileDescriptor0, []int{1} }

func (m *TcpResponseLogin) GetResult() bool {
	if m != nil && m.Result != nil {
		return *m.Result
	}
	return false
}

func (m *TcpResponseLogin) GetUid() int32 {
	if m != nil && m.Uid != nil {
		return *m.Uid
	}
	return 0
}

func (m *TcpResponseLogin) GetUdpPort() int32 {
	if m != nil && m.UdpPort != nil {
		return *m.UdpPort
	}
	return 0
}

func init() {
	proto.RegisterType((*TcpLogin)(nil), "PBLogin.TcpLogin")
	proto.RegisterType((*TcpResponseLogin)(nil), "PBLogin.TcpResponseLogin")
}

func init() { proto.RegisterFile("PBLogin.proto", fileDescriptor0) }

var fileDescriptor0 = []byte{
	// 127 bytes of a gzipped FileDescriptorProto
	0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0xff, 0xe2, 0xe2, 0x0d, 0x70, 0xf2, 0xc9,
	0x4f, 0xcf, 0xcc, 0xd3, 0x2b, 0x28, 0xca, 0x2f, 0xc9, 0x17, 0x62, 0x87, 0x72, 0x95, 0x24, 0xb9,
	0x38, 0x42, 0x92, 0x0b, 0xc0, 0x6c, 0x21, 0x5e, 0x2e, 0xd6, 0x92, 0xfc, 0xec, 0xd4, 0x3c, 0x09,
	0x46, 0x05, 0x26, 0x0d, 0x4e, 0x25, 0x07, 0x2e, 0x81, 0x90, 0xe4, 0x82, 0xa0, 0xd4, 0xe2, 0x82,
	0xfc, 0xbc, 0xe2, 0x54, 0x88, 0x12, 0x3e, 0x2e, 0xb6, 0xa2, 0xd4, 0xe2, 0xd2, 0x9c, 0x12, 0xb0,
	0x1a, 0x0e, 0x21, 0x6e, 0x2e, 0xe6, 0xd2, 0xcc, 0x14, 0x09, 0x26, 0x05, 0x26, 0x0d, 0x56, 0x21,
	0x7e, 0x2e, 0xf6, 0xd2, 0x94, 0x82, 0x80, 0xfc, 0xa2, 0x12, 0x09, 0x66, 0x90, 0x80, 0x13, 0x93,
	0x07, 0x33, 0x20, 0x00, 0x00, 0xff, 0xff, 0xf3, 0x8f, 0x17, 0x83, 0x79, 0x00, 0x00, 0x00,
}
