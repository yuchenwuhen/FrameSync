// Code generated by protoc-gen-go. DO NOT EDIT.
// source: PBMatch.proto

/*
Package PBMatch is a generated protocol buffer package.

It is generated from these files:
	PBMatch.proto

It has these top-level messages:
	TcpRequestMatch
	TcpCancelMatch
	TcpResponseRequestMatch
	TcpResponseCancelMatch
*/
package PBMatch

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

type TcpRequestMatch struct {
	Uid              *int32 `protobuf:"varint,1,req,name=uid" json:"uid,omitempty"`
	RoleID           *int32 `protobuf:"varint,2,req,name=roleID" json:"roleID,omitempty"`
	XXX_unrecognized []byte `json:"-"`
}

func (m *TcpRequestMatch) Reset()                    { *m = TcpRequestMatch{} }
func (m *TcpRequestMatch) String() string            { return proto.CompactTextString(m) }
func (*TcpRequestMatch) ProtoMessage()               {}
func (*TcpRequestMatch) Descriptor() ([]byte, []int) { return fileDescriptor0, []int{0} }

func (m *TcpRequestMatch) GetUid() int32 {
	if m != nil && m.Uid != nil {
		return *m.Uid
	}
	return 0
}

func (m *TcpRequestMatch) GetRoleID() int32 {
	if m != nil && m.RoleID != nil {
		return *m.RoleID
	}
	return 0
}

type TcpCancelMatch struct {
	Uid              *int32 `protobuf:"varint,1,req,name=uid" json:"uid,omitempty"`
	XXX_unrecognized []byte `json:"-"`
}

func (m *TcpCancelMatch) Reset()                    { *m = TcpCancelMatch{} }
func (m *TcpCancelMatch) String() string            { return proto.CompactTextString(m) }
func (*TcpCancelMatch) ProtoMessage()               {}
func (*TcpCancelMatch) Descriptor() ([]byte, []int) { return fileDescriptor0, []int{1} }

func (m *TcpCancelMatch) GetUid() int32 {
	if m != nil && m.Uid != nil {
		return *m.Uid
	}
	return 0
}

type TcpResponseRequestMatch struct {
	XXX_unrecognized []byte `json:"-"`
}

func (m *TcpResponseRequestMatch) Reset()                    { *m = TcpResponseRequestMatch{} }
func (m *TcpResponseRequestMatch) String() string            { return proto.CompactTextString(m) }
func (*TcpResponseRequestMatch) ProtoMessage()               {}
func (*TcpResponseRequestMatch) Descriptor() ([]byte, []int) { return fileDescriptor0, []int{2} }

type TcpResponseCancelMatch struct {
	XXX_unrecognized []byte `json:"-"`
}

func (m *TcpResponseCancelMatch) Reset()                    { *m = TcpResponseCancelMatch{} }
func (m *TcpResponseCancelMatch) String() string            { return proto.CompactTextString(m) }
func (*TcpResponseCancelMatch) ProtoMessage()               {}
func (*TcpResponseCancelMatch) Descriptor() ([]byte, []int) { return fileDescriptor0, []int{3} }

func init() {
	proto.RegisterType((*TcpRequestMatch)(nil), "PBMatch.TcpRequestMatch")
	proto.RegisterType((*TcpCancelMatch)(nil), "PBMatch.TcpCancelMatch")
	proto.RegisterType((*TcpResponseRequestMatch)(nil), "PBMatch.TcpResponseRequestMatch")
	proto.RegisterType((*TcpResponseCancelMatch)(nil), "PBMatch.TcpResponseCancelMatch")
}

func init() { proto.RegisterFile("PBMatch.proto", fileDescriptor0) }

var fileDescriptor0 = []byte{
	// 127 bytes of a gzipped FileDescriptorProto
	0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0xff, 0xe2, 0xe2, 0x0d, 0x70, 0xf2, 0x4d,
	0x2c, 0x49, 0xce, 0xd0, 0x2b, 0x28, 0xca, 0x2f, 0xc9, 0x17, 0x62, 0x87, 0x72, 0x95, 0xf4, 0xb8,
	0xf8, 0x43, 0x92, 0x0b, 0x82, 0x52, 0x0b, 0x4b, 0x53, 0x8b, 0x4b, 0xc0, 0x42, 0x42, 0xdc, 0x5c,
	0xcc, 0xa5, 0x99, 0x29, 0x12, 0x8c, 0x0a, 0x4c, 0x1a, 0xac, 0x42, 0x7c, 0x5c, 0x6c, 0x45, 0xf9,
	0x39, 0xa9, 0x9e, 0x2e, 0x12, 0x4c, 0x20, 0xbe, 0x92, 0x2c, 0x17, 0x5f, 0x48, 0x72, 0x81, 0x73,
	0x62, 0x5e, 0x72, 0x6a, 0x0e, 0xa6, 0x72, 0x25, 0x49, 0x2e, 0x71, 0xb0, 0x71, 0xc5, 0x05, 0xf9,
	0x79, 0xc5, 0xa9, 0xc8, 0xc6, 0x2a, 0x49, 0x70, 0x89, 0x21, 0x49, 0x21, 0x99, 0xe0, 0xc4, 0xe4,
	0xc1, 0x0c, 0x08, 0x00, 0x00, 0xff, 0xff, 0xa1, 0x44, 0xfe, 0xa3, 0xa0, 0x00, 0x00, 0x00,
}